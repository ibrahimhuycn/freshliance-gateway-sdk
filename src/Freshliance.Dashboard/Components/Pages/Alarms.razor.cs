using Freshliance.Dashboard.Services;
using FreshlianceGateway.Sdk;
using FreshlianceGateway.Sdk.Models.Data;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Freshliance.Dashboard.Components.Pages;

public partial class Alarms
{
    [Inject] private IDeviceService DeviceService { get; set; } = null!;
    [Inject] private IDeviceDataService DeviceDataService { get; set; } = null!;
    [Inject] private UserPreferenceService Preferences { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private MudDataGrid<AlarmDataResponse> _grid = null!;
    private bool _isLoading;
    private string? _error;
    private int _totalItems;
    private List<DeviceInfoResponse> _devices = [];
    private int? _selectedRecordId;
    private bool _loadingDevices = true;

    private int? _alarmPropertyFilter;
    private int? _alarmTypeFilter;
    private int? _handleStatusFilter;
    private int? _probeTypeFilter;
    private DateRange? _dateRange;

    protected override async Task OnInitializedAsync()
    {
        await LoadDevicesAsync();
    }

    private async Task LoadDevicesAsync()
    {
        _loadingDevices = true;
        try
        {
            var response = await DeviceService.GetPageAsync(new GetDevicePageRequest { PageSize = 50, PageNum = 1 });
            response.EnsureSuccess();
            _devices = (response.Data?.Rows ?? []).Select(r => r.DeviceInfo).ToList();
        }
        catch (Exception ex)
        {
            _error = ex.Message;
        }
        finally
        {
            _loadingDevices = false;
        }
    }

    private async Task OnDeviceSelected(int? recordId)
    {
        _selectedRecordId = recordId;
        if (recordId is > 0)
        {
            _isLoading = true;
            _error = null;
            await _grid.ReloadServerData();
        }
        else
        {
            _totalItems = 0;
        }
    }

    private async Task ReloadAsync()
    {
        if (_selectedRecordId is not > 0) return;

        _isLoading = true;
        _error = null;
        await _grid.ReloadServerData();
    }

    private async Task<GridData<AlarmDataResponse>> LoadGridData(GridState<AlarmDataResponse> state, CancellationToken cancellationToken)
    {
        _error = null;

        if (_selectedRecordId is not > 0)
        {
            _isLoading = false;
            return new GridData<AlarmDataResponse> { Items = [], TotalItems = 0 };
        }

        try
        {
            var request = new GetAlarmDataRequest
            {
                RecordId = _selectedRecordId,
                AlarmProperty = _alarmPropertyFilter,
                ProbeType = _probeTypeFilter,
                AlarmType = _alarmTypeFilter,
                HandleStatus = _handleStatusFilter,
                AlarmStartTime = _dateRange?.Start is not null
                    ? new DateTimeOffset(_dateRange.Start.Value, TimeSpan.Zero).ToUnixTimeMilliseconds()
                    : null,
                AlarmEndTime = _dateRange?.End is not null
                    ? new DateTimeOffset(_dateRange.End.Value, TimeSpan.Zero).ToUnixTimeMilliseconds()
                    : null,
                PageNum = state.Page + 1,
                PageSize = state.PageSize,
            };

            var response = await DeviceDataService.GetAlarmPageAsync(request);
            response.EnsureSuccess();

            _totalItems = response.Data?.Total ?? 0;
            _isLoading = false;

            return new GridData<AlarmDataResponse>
            {
                Items = response.Data?.Rows ?? [],
                TotalItems = _totalItems,
            };
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            _totalItems = 0;
            _isLoading = false;
            return new GridData<AlarmDataResponse> { Items = [], TotalItems = 0 };
        }
    }

    private async Task OnAlarmPropertyChanged(int? value)
    {
        _alarmPropertyFilter = value;
        await ReloadAsync();
    }

    private async Task OnAlarmTypeChanged(int? value)
    {
        _alarmTypeFilter = value;
        await ReloadAsync();
    }

    private async Task OnHandleStatusChanged(int? value)
    {
        _handleStatusFilter = value;
        await ReloadAsync();
    }

    private async Task OnProbeTypeChanged(int? value)
    {
        _probeTypeFilter = value;
        await ReloadAsync();
    }

    private async Task OnDateRangeChanged(DateRange? range)
    {
        _dateRange = range;
        await ReloadAsync();
    }

    private static readonly Func<AlarmDataResponse, int, string> RowStyleFunc =
        (item, _) => item.HandleStatus == 1 ? "background-color: #FFEBEE" : string.Empty;

    private static string GetProbeTypeLabel(int probeType) => probeType switch
    {
        0 => "Built-in",
        1 => "External 1",
        2 => "External 2",
        _ => $"Probe {probeType}",
    };

    private static string GetAlarmPropertyIcon(int alarmProperty) => alarmProperty switch
    {
        1 => Icons.Material.Filled.Thermostat,
        2 => Icons.Material.Filled.WaterDrop,
        3 => Icons.Material.Filled.LightMode,
        4 => Icons.Material.Filled.Co2,
        _ => Icons.Material.Filled.DeviceUnknown,
    };

    private string GetAlarmPropertyUnit(int alarmProperty) => alarmProperty switch
    {
        1 => Preferences.UserInfo?.TemperatureUnit == 2 ? "°F" : "°C",
        2 => "%",
        3 => "lux",
        4 => "ppm",
        _ => "",
    };

    private static Color GetHandleStatusColor(int handleStatus) => handleStatus switch
    {
        1 => Color.Warning,
        2 => Color.Success,
        3 => Color.Default,
        _ => Color.Default,
    };

    private static string GetHandleStatusLabel(int handleStatus) => handleStatus switch
    {
        1 => "Processing",
        2 => "Processed",
        3 => "Ignored",
        _ => "Unknown",
    };

    private static Color GetAlarmZoneColor(string zone) =>
        zone == "H" ? Color.Error : zone == "L" ? Color.Info : Color.Default;

    private string FormatAlarmThreshold(AlarmDataResponse alarm)
    {
        if (alarm.AlarmProperty == 1)
            return Preferences.FormatTemperature(alarm.AlarmThreshold);

        return $"{alarm.AlarmThreshold:F1} {GetAlarmPropertyUnit(alarm.AlarmProperty)}";
    }

    private string FormatPropertyValue(AlarmDataResponse alarm) =>
        $"{alarm.PropertyValue} {GetAlarmPropertyUnit(alarm.AlarmProperty)}";

    private string FormatAlarmTime(long alarmTime)
    {
        return Preferences.FormatDateTime(alarmTime);
    }

    private void NavigateToDevice(int recordId) =>
        Navigation.NavigateTo($"/device/{recordId}");
}
