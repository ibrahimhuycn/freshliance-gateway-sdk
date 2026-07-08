using Microsoft.AspNetCore.Components;
using MudBlazor;
using Freshliance.Dashboard.Components.Shared;
using Freshliance.Dashboard.Services;
using FreshlianceGateway.Sdk.Models.Data;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Models.Group;
using FreshlianceGateway.Sdk.Services;
using Timer = System.Timers.Timer;

namespace Freshliance.Dashboard.Components.Pages;

public partial class Dashboard : IDisposable
{
    [Inject] private IDeviceService DeviceService { get; set; } = null!;
    [Inject] private IGroupService GroupService { get; set; } = null!;
    [Inject] private UserPreferenceService UserPreferenceService { get; set; } = null!;
    [Inject] private DeviceStateService DeviceStateService { get; set; } = null!;
    [Inject] private PollingService PollingService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private bool _isLoading = true;
    private string? _error;
    private DashboardSnapshot _snapshot;
    private List<TreeItemData<GroupTreeItemData>> _groupTreeItems = [];
    private DateTimeOffset _lastUpdated;
    private string _lastUpdatedText = "just now";
    private bool _autoRefresh = true;
    private bool _disposed;
    private Timer? _timer;

    private int TotalDevices => _snapshot.TotalDevices;
    private int OnlineCount => _snapshot.OnlineCount;
    private int OfflineCount => _snapshot.OfflineCount;
    private int AlarmCount => _snapshot.AlarmCount;
    private List<AlarmDataResponse> RecentAlarms => _snapshot.RecentAlarms;

    private double OnlinePercent => TotalDevices > 0 ? Math.Round((double)OnlineCount / TotalDevices * 100, 1) : 0;
    private double OfflinePercent => TotalDevices > 0 ? Math.Round((double)OfflineCount / TotalDevices * 100, 1) : 0;
    private double AlarmPercent => TotalDevices > 0 ? Math.Round((double)AlarmCount / TotalDevices * 100, 1) : 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            PollingService.SnapshotUpdated += OnSnapshotUpdated;
            await LoadDataAsync();
            StartTimer();
        }
        catch (Exception ex)
        {
            _error = $"Failed to load dashboard: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task LoadDataAsync()
    {
        var deviceTask = DeviceService.GetPageAsync(new GetDevicePageRequest { PageSize = 50, PageNum = 1 });
        var groupTask = GroupService.GetTreeAsync();

        await Task.WhenAll(deviceTask, groupTask);

        var deviceResponse = deviceTask.Result;
        deviceResponse.EnsureSuccess();
        var devices = deviceResponse.Data?.Rows ?? [];
        DeviceStateService.SetCache(devices);

        var totalDevices = deviceResponse.Data?.Total ?? devices.Count;
        var onlineCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 1);
        var offlineCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 2);
        var abnormalCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 3);
        var inactiveCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 0);
        var alarmCount = devices.Count(d => d.DeviceInfo.AlarmStatus == 2);

        var groupResponse = groupTask.Result;
        groupResponse.EnsureSuccess();
        _groupTreeItems = ConvertToTreeItems(groupResponse.Data ?? []);

        _lastUpdated = DateTimeOffset.UtcNow;
        UpdateLastUpdatedText();

        _snapshot = new DashboardSnapshot(
            totalDevices,
            onlineCount,
            offlineCount,
            alarmCount,
            abnormalCount,
            inactiveCount,
            _lastUpdated,
            []
        );
    }

    private void StartTimer()
    {
        _timer = new Timer(10000);
        _timer.Elapsed += (_, _) =>
        {
            if (_disposed) return;
            InvokeAsync(() => { UpdateLastUpdatedText(); StateHasChanged(); });
        };
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void UpdateLastUpdatedText()
    {
        var elapsed = DateTimeOffset.UtcNow - _lastUpdated;
        _lastUpdatedText = elapsed.TotalSeconds switch
        {
            < 10 => "just now",
            < 60 => $"{(int)elapsed.TotalSeconds}s ago",
            < 3600 => $"{(int)elapsed.TotalMinutes}m ago",
            _ => $"{(int)elapsed.TotalHours}h ago"
        };
    }

    private void OnSnapshotUpdated(DashboardSnapshot snapshot)
    {
        if (!_autoRefresh || _disposed) return;

        InvokeAsync(() =>
        {
            _snapshot = snapshot;
            _lastUpdated = snapshot.LastUpdated;
            UpdateLastUpdatedText();
            StateHasChanged();
        });
    }

    private string GetDeviceName(int recordId)
    {
        var device = DeviceStateService.GetByRecordId(recordId);
        return device?.DeviceInfo.DeviceName ?? $"Device #{recordId}";
    }

    private static string GetAlarmPropertyName(int alarmProperty)
    {
        return alarmProperty switch
        {
            1 => "Temperature",
            2 => "Humidity",
            3 => "Light",
            4 => "Shock",
            5 => "Tilt",
            _ => $"Property {alarmProperty}"
        };
    }

    private static (Color Color, string Label) GetAlarmTypeBadge(int alarmType)
    {
        return alarmType switch
        {
            1 => (Color.Error, "H"),
            2 => (Color.Info, "L"),
            _ => (Color.Default, "-")
        };
    }

    private static (Color Color, string Label) GetHandleStatusBadge(int handleStatus)
    {
        return handleStatus switch
        {
            0 => (Color.Warning, "Unhandled"),
            1 => (Color.Info, "In Progress"),
            2 => (Color.Success, "Handled"),
            _ => (Color.Default, "Unknown")
        };
    }

    private void NavigateToDevices()
    {
        NavigationManager.NavigateTo("/devices");
    }

    private List<TreeItemData<GroupTreeItemData>> ConvertToTreeItems(List<GroupTreeNodeResponse> nodes)
    {
        return nodes.Select(ConvertNode).ToList();
    }

    private static TreeItemData<GroupTreeItemData> ConvertNode(GroupTreeNodeResponse node)
    {
        var data = new GroupTreeItemData
        {
            GroupId = node.GroupId,
            GroupName = node.GroupName,
            DeviceCount = node.DeviceGroupCount?.DeviceCount ?? 0
        };

        var children = node.SubDeviceGroupList is { Count: > 0 }
            ? node.SubDeviceGroupList.Select(ConvertNode).ToList()
            : null;

        return new TreeItemData<GroupTreeItemData>
        {
            Value = data,
            Text = $"{data.GroupName} ({data.DeviceCount})",
            Children = children
        };
    }

    public void Dispose()
    {
        _disposed = true;
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
        PollingService.SnapshotUpdated -= OnSnapshotUpdated;
    }

    public sealed record GroupTreeItemData
    {
        public int GroupId { get; init; }
        public string GroupName { get; init; } = "";
        public int DeviceCount { get; init; }
    }
}
