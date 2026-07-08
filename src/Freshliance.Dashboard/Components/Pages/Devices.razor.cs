using Freshliance.Dashboard.Services;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Freshliance.Dashboard.Components.Pages;

public partial class Devices : IDisposable
{
    [Inject] private ILogger<Devices> Logger { get; set; } = null!;
    [Inject] private IDeviceService DeviceService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private DeviceStateService DeviceState { get; set; } = null!;
    [Inject] private UserPreferenceService Preferences { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private MudDataGrid<DevicePageItemResponse> _grid = null!;
    private string _keyword = string.Empty;
    private int? _status;
    private int? _productType;
    private int? _alarmStatus;
    private bool _isLoading = true;
    private int _totalItems;
    private int _cachedPage = -1;
    private int _cachedPageSize;
    private bool _filtersDirty;
    private CancellationTokenSource? _debounceCts;
    private List<DevicePageItemResponse> _currentPageItems = [];

    protected override async Task OnInitializedAsync()
    {
        Logger.LogInformation("Devices page initializing");
        await LoadInitialDataAsync();
        Logger.LogInformation("Devices page initialized — items={TotalItems}", _totalItems);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
            Logger.LogInformation("Devices page first render — loading={IsLoading}, items={TotalItems}", _isLoading, _totalItems);
    }

    private async Task LoadInitialDataAsync()
    {
        _isLoading = true;
        try
        {
            var request = BuildRequest(1, 20);
            Logger.LogDebug("Devices initial load — pageNum={PageNum}, pageSize={PageSize}, status={Status}, productType={ProductType}, alarmStatus={AlarmStatus}",
                request.PageNum, request.PageSize, request.DeviceStatus, request.ProductType, request.AlarmStatus);
            var response = await DeviceService.GetPageAsync(request);
            response.EnsureSuccess();

            _currentPageItems = response.Data?.Rows ?? [];
            _totalItems = response.Data?.Total ?? 0;
            _cachedPage = 0;
            _cachedPageSize = 20;
            Logger.LogInformation("Devices initial load done — code={Code}, total={Total}, rows={Rows}", response.Code, _totalItems, _currentPageItems.Count);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Devices initial load failed");
            Snackbar.Add(ex.Message, Severity.Error);
            _currentPageItems = [];
            _totalItems = 0;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task<GridData<DevicePageItemResponse>> LoadGridData(GridState<DevicePageItemResponse> state, CancellationToken cancellationToken)
    {
        if (state.Page == _cachedPage && state.PageSize == _cachedPageSize && !_filtersDirty)
        {
            Logger.LogDebug("Devices grid cache hit — page={Page}, pageSize={PageSize}, cachedPage={CachedPage}, cachedSize={CachedSize}",
                state.Page, state.PageSize, _cachedPage, _cachedPageSize);
            return new GridData<DevicePageItemResponse> { Items = _currentPageItems, TotalItems = _totalItems };
        }

        Logger.LogDebug("Devices grid cache miss — page={Page}/{CachedPage}, size={PageSize}/{CachedSize}, dirty={Dirty}",
            state.Page, _cachedPage, state.PageSize, _cachedPageSize, _filtersDirty);

        _filtersDirty = false;
        try
        {
            var request = BuildRequest(state.Page + 1, state.PageSize);
            var response = await DeviceService.GetPageAsync(request);
            response.EnsureSuccess();

            var data = response.Data;
            _currentPageItems = data?.Rows ?? [];
            _totalItems = data?.Total ?? 0;
            _cachedPage = state.Page;
            _cachedPageSize = state.PageSize;

            Logger.LogInformation("Devices grid load done — total={Total}, rows={Rows}", _totalItems, _currentPageItems.Count);

            return new GridData<DevicePageItemResponse> { Items = _currentPageItems, TotalItems = _totalItems };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Devices grid load failed");
            Snackbar.Add(ex.Message, Severity.Error);
            _currentPageItems = [];
            _totalItems = 0;
            return new GridData<DevicePageItemResponse> { Items = [], TotalItems = 0 };
        }
    }

    private async Task ReloadAsync()
    {
        Logger.LogDebug("Devices reload — grid={HasGrid}, items={Items}", _grid is not null, _totalItems);
        if (_grid is not null && _totalItems > 0)
        {
            _filtersDirty = true;
            await _grid.ReloadServerData();
        }
        else
        {
            await LoadInitialDataAsync();
        }
    }

    private async Task OnKeywordChangedAsync(string value)
    {
        Logger.LogDebug("Devices keyword filter changed — '{Keyword}'", value);
        _keyword = value;
        _filtersDirty = true;
        _debounceCts?.Cancel();
        _debounceCts = new CancellationTokenSource();
        try
        {
            await Task.Delay(500, _debounceCts.Token);
            if (_grid is not null)
                await _grid.ReloadServerData();
        }
        catch (TaskCanceledException)
        {
        }
    }

    private async Task OnStatusChangedAsync(int? value)
    {
        Logger.LogDebug("Devices status filter changed — {Status}", value);
        _status = value;
        _filtersDirty = true;
        if (_grid is not null)
            await _grid.ReloadServerData();
    }

    private async Task OnProductTypeChangedAsync(int? value)
    {
        Logger.LogDebug("Devices productType filter changed — {ProductType}", value);
        _productType = value;
        _filtersDirty = true;
        if (_grid is not null)
            await _grid.ReloadServerData();
    }

    private async Task OnAlarmStatusChangedAsync(int? value)
    {
        Logger.LogDebug("Devices alarmStatus filter changed — {AlarmStatus}", value);
        _alarmStatus = value;
        _filtersDirty = true;
        if (_grid is not null)
            await _grid.ReloadServerData();
    }

    private void NavigateToDevice(DevicePageItemResponse item)
    {
        DeviceState.SetCache(_currentPageItems);
        Navigation.NavigateTo($"/device/{item.DeviceInfo.RecordId}");
    }

    private void OnRowClick(DataGridRowClickEventArgs<DevicePageItemResponse> args)
    {
        if (args.Item is not null)
        {
            NavigateToDevice(args.Item);
        }
    }

    private Color BatteryColor(int? power) => power switch
    {
        null => Color.Default,
        <= 10 => Color.Error,
        <= 25 => Color.Warning,
        _ => Color.Success
    };

    private GetDevicePageRequest BuildRequest(int pageNum, int pageSize) => new()
    {
        PageNum = pageNum,
        PageSize = pageSize,
        Keyword = string.IsNullOrWhiteSpace(_keyword) ? null : _keyword.Trim(),
        DeviceStatus = _status,
        ProductType = _productType,
        AlarmStatus = _alarmStatus
    };

    public void Dispose()
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
    }
}
