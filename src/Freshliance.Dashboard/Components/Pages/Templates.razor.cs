using Microsoft.AspNetCore.Components;
using MudBlazor;
using Freshliance.Dashboard.Components.Shared;
using FreshlianceGateway.Sdk;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Models.Template;
using FreshlianceGateway.Sdk.Services;

namespace Freshliance.Dashboard.Components.Pages;

public partial class Templates
{
    [Inject] private IConfigTemplateService ConfigTemplateService { get; set; } = null!;
    [Inject] private IDeviceService DeviceService { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private MudDataGrid<TemplatePageItemResponse> _grid = null!;
    private List<CategoryResponse> _categories = [];
    private List<TemplatePageItemResponse> _currentItems = [];
    private int _totalItems;
    private bool _loading;
    private bool _initialLoad = true;
    private string? _error;
    private TemplatePageItemResponse? _expandedItem;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var response = await DeviceService.GetCategoriesAsync();
            response.EnsureSuccess();
            _categories = response.Data ?? [];
        }
        catch (FreshlianceException ex)
        {
            _error = ex.Message;
        }
    }

    private async Task<GridData<TemplatePageItemResponse>> LoadData(GridState<TemplatePageItemResponse> state, CancellationToken token)
    {
        _loading = true;
        _error = null;
        try
        {
            var request = new GetTemplatePageRequest
            {
                PageNum = state.Page + 1,
                PageSize = state.PageSize
            };
            var response = await ConfigTemplateService.GetPageAsync(request);
            response.EnsureSuccess();
            var data = response.Data;
            _currentItems = data?.Rows ?? [];
            _totalItems = data?.Total ?? 0;
            _initialLoad = false;
            return new GridData<TemplatePageItemResponse>
            {
                Items = _currentItems,
                TotalItems = _totalItems
            };
        }
        catch (FreshlianceException ex)
        {
            _error = ex.Message;
            _initialLoad = false;
            return new GridData<TemplatePageItemResponse> { Items = [], TotalItems = 0 };
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OpenCreateDialog()
    {
        var parameters = new DialogParameters
        {
            ["Categories"] = _categories,
            ["ExistingDetail"] = null
        };
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
            CloseButton = true
        };
        var dialog = await DialogService.ShowAsync<TemplateDialog>("Create Template", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled && result.Data is CreateTemplateRequest createRequest)
            await CreateTemplate(createRequest);
    }

    private async Task OpenEditDialog(TemplatePageItemResponse item)
    {
        try
        {
            var detailReq = new GetTemplateDetailRequest { ConfigId = item.ConfigId };
            var detailResp = await ConfigTemplateService.GetAsync(detailReq);
            detailResp.EnsureSuccess();

            var parameters = new DialogParameters
            {
                ["Categories"] = _categories,
                ["ExistingDetail"] = detailResp.Data,
                ["ConfigId"] = item.ConfigId
            };
            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true,
                CloseButton = true
            };
            var dialog = await DialogService.ShowAsync<TemplateDialog>("Edit Template", parameters, options);
            var result = await dialog.Result;
            if (result is not null && !result.Canceled && result.Data is UpdateTemplateRequest updateRequest)
                await UpdateTemplate(updateRequest);
        }
        catch (FreshlianceException ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task CreateTemplate(CreateTemplateRequest request)
    {
        try
        {
            var response = await ConfigTemplateService.CreateAsync(request);
            response.EnsureSuccess();
            Snackbar.Add("Template created successfully.", Severity.Success);
            await _grid.ReloadServerData();
        }
        catch (FreshlianceException ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task UpdateTemplate(UpdateTemplateRequest request)
    {
        try
        {
            var response = await ConfigTemplateService.UpdateAsync(request);
            response.EnsureSuccess();
            Snackbar.Add("Template updated successfully.", Severity.Success);
            await _grid.ReloadServerData();
        }
        catch (FreshlianceException ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task DeleteTemplate(TemplatePageItemResponse item)
    {
        var parameters = new DialogParameters
        {
            ["Title"] = "Delete Template",
            ["Message"] = $"Are you sure you want to delete \"{item.TemplateName}\"?",
            ["ConfirmText"] = "Delete"
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm", parameters, options);
        var result = await dialog.Result;
        if (result is null || result.Canceled || result.Data is not true)
            return;

        try
        {
            var request = new DeleteTemplateRequest { ConfigId = item.ConfigId };
            var response = await ConfigTemplateService.DeleteAsync(request);
            response.EnsureSuccess();
            Snackbar.Add("Template deleted successfully.", Severity.Success);
            _expandedItem = null;
            await _grid.ReloadServerData();
        }
        catch (FreshlianceException ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private void ToggleDetail(TemplatePageItemResponse item)
    {
        _expandedItem = _expandedItem?.ConfigId == item.ConfigId ? null : item;
    }

    private static int GetAlarmCount(TemplatePageItemResponse item)
    {
        if (item.SensorConfigProbeList is null) return 0;
        return item.SensorConfigProbeList
            .Where(p => p.SensorAlarmList is not null)
            .SelectMany(p => p.SensorAlarmList!)
            .Count();
    }

    private static List<(TemplateAlarmResponse Alarm, string ProbeLabel)> GetFlatAlarms(TemplatePageItemResponse item)
    {
        var result = new List<(TemplateAlarmResponse, string)>();
        if (item.SensorConfigProbeList is null) return result;
        foreach (var probe in item.SensorConfigProbeList)
        {
            var label = GetProbeLabel(probe.ProbeType);
            if (probe.SensorAlarmList is not null)
            {
                foreach (var alarm in probe.SensorAlarmList)
                    result.Add((alarm, label));
            }
        }
        return result;
    }

    private static string GetProbeLabel(int probeType) => probeType switch
    {
        0 => "Built-in",
        1 => "External 1",
        2 => "External 2",
        _ => $"Probe {probeType}"
    };

    private static string GetAlarmPropertyLabel(int? property) => property switch
    {
        1 => "Temperature",
        2 => "Humidity",
        3 => "Illumination",
        4 => "CO₂",
        _ => "—"
    };

    private static string GetAlarmTypeLabel(int? type) => type switch
    {
        1 => "Low",
        2 => "High",
        _ => "—"
    };

    private static string GetAlarmWayLabel(int? way) => way switch
    {
        1 => "Single",
        2 => "Cumulative",
        _ => "—"
    };

    private static string GetAlarmUnit(int? property) => property switch
    {
        1 => "°C",
        2 => "%",
        3 => "lux",
        4 => "ppm",
        _ => ""
    };
}

public class AlarmConfigRow
{
    public int? ProbeType { get; set; }
    public string AlarmZone { get; set; } = "H";
    public int? AlarmProperty { get; set; }
    public int? AlarmType { get; set; }
    public double? AlarmThreshold { get; set; }
    public int? AlarmDelay { get; set; }
    public int? AlarmWay { get; set; }
}
