using System.Text;
using System.Text.Json;
using ApexCharts;
using Freshliance.Dashboard.Services;
using FreshlianceGateway.Sdk;
using FreshlianceGateway.Sdk.Models.Data;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MudBlazor;

namespace Freshliance.Dashboard.Components.Pages;

public partial class DeviceDetail
{
    [Parameter] public int RecordId { get; set; }

    [Inject] private IDeviceDataService DeviceDataService { get; set; } = null!;
    [Inject] private IDeviceService DeviceService { get; set; } = null!;
    [Inject] private DeviceStateService DeviceStateService { get; set; } = null!;
    [Inject] private UserPreferenceService Preferences { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private ILogger<DeviceDetail> Logger { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private static readonly Dictionary<int, RenderFragment> ProbeEntries = new()
    {
        [0] = _ => { },
        [1] = _ => { },
        [2] = _ => { }
    };

    private DeviceInfoResponse? _device;
    private bool _isLoading = true;
    private bool _isProbeDataLoading;
    private string? _error;

    private readonly Dictionary<int, List<DeviceDataResponse>> _probeData = new();
    private readonly Dictionary<int, List<ChartSeries>> _chartSeries = new();
    private readonly Dictionary<int, SubDeviceDataResponse?> _latestReadings = new();
    private readonly Dictionary<int, bool> _probeHasData = new();

    private List<AlarmDataResponse> _alarms = [];
    private long[] _timeRange = [];

    private List<BreadcrumbItem> BreadcrumbItems => _device is null ? [] :
    [
        new BreadcrumbItem("Dashboard", href: "/", icon: Icons.Material.Filled.Dashboard),
        new BreadcrumbItem("Devices", href: "/devices", icon: Icons.Material.Filled.Devices),
        new BreadcrumbItem(_device.DeviceName, href: null, disabled: true)
    ];

    private MudBlazor.Color BatteryColor => _device?.DevicePower switch
    {
        <= 10 => MudBlazor.Color.Error,
        <= 25 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Success
    };

    protected override async Task OnInitializedAsync()
    {
        if (_device is not null) return; // already loaded on previous render
        try
        {
            _isLoading = true;
            Logger.LogInformation("DeviceDetail init — RecordId={RecordId}", RecordId);

            var cached = DeviceStateService.GetByRecordId(RecordId);
            if (cached is not null)
            {
                _device = cached.DeviceInfo;
                Logger.LogInformation("DeviceDetail loaded from cache — name={Name}", _device.DeviceName);
            }
            else
            {
                Logger.LogInformation("DeviceDetail cache miss — fetching all devices");
                var deviceResponse = await DeviceService.GetPageAsync(new GetDevicePageRequest { PageSize = 50 });
                deviceResponse.EnsureSuccess();
                var found = deviceResponse.Data?.Rows?.Find(d => d.DeviceInfo.RecordId == RecordId);
                if (found is not null)
                {
                    _device = found.DeviceInfo;
                    Logger.LogInformation("DeviceDetail found in API — name={Name}", _device.DeviceName);
                }
                else
                {
                    Logger.LogWarning("DeviceDetail not found — RecordId={RecordId} not in {Count} devices", RecordId, deviceResponse.Data?.Rows?.Count ?? 0);
                }
            }

            if (_device is null)
            {
                _error = "Device not found.";
                return;
            }

            var end = DateTimeOffset.Now;
            var start = end.AddDays(-7);
            _timeRange = [start.ToUnixTimeMilliseconds(), end.ToUnixTimeMilliseconds()];
            Logger.LogInformation("DeviceDetail loading probe data — range={Start} to {End}", start, end);

            await LoadProbeDataAsync();
            Logger.LogInformation("DeviceDetail done — probes 0={P0}, 1={P1}, 2={P2}, alarms={Alarms}",
                _probeHasData.GetValueOrDefault(0), _probeHasData.GetValueOrDefault(1), _probeHasData.GetValueOrDefault(2), _alarms.Count);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "DeviceDetail failed");
            _error = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task LoadProbeDataAsync()
    {
        _isProbeDataLoading = true;

        var alarmTask = DeviceDataService.GetAlarmPageAsync(new GetAlarmDataRequest
        {
            RecordId = RecordId,
            PageSize = 50
        });

        for (var i = 0; i < 3; i++)
        {
            var allRows = new List<DeviceDataResponse>();
            var total = 0;
            const int maxPages = 20;

            for (var page = 1; page <= maxPages; page++)
            {
                try
                {
                    var request = new GetDeviceDataRequest
                    {
                        RecordId = RecordId,
                        ProbeType = i,
                        PageSize = 50,
                        PageNum = page,
                        DataTime = _timeRange
                    };
                    var response = await DeviceDataService.GetDataPageAsync(request);
                    response.EnsureSuccess();
                    var rows = response.Data?.Rows ?? [];
                    total = response.Data?.Total ?? 0;
                    allRows.AddRange(rows);
                    if (allRows.Count >= total || rows.Count == 0)
                        break;
                    if (page > 1) await Task.Delay(200);
                }
                catch (FreshlianceException)
                {
                    break; // rate limited, keep what we have
                }
            }

            _probeData[i] = allRows;
            _chartSeries[i] = BuildChartSeries(allRows);
            _probeHasData[i] = allRows.Count > 0;

            var latest = allRows.Count > 0
                ? allRows.OrderByDescending(d => d.DataTime).First()
                : null;

            if (latest is not null)
            {
                _latestReadings[i] = new SubDeviceDataResponse
                {
                    Temperature = latest.Temperature,
                    Humidity = latest.Humidity,
                    Light = latest.Light,
                    Co2 = latest.Co2,
                    DataTime = latest.DataTime,
                    Status = latest.Status
                };
            }
            else
            {
                _latestReadings[i] = null;
            }
        }

        var alarmResult = await alarmTask;
        alarmResult.EnsureSuccess();
        _alarms = alarmResult.Data?.Rows ?? [];

        _isProbeDataLoading = false;
    }

    private static List<ChartSeries> BuildChartSeries(List<DeviceDataResponse> data)
    {
        var sorted = data.OrderBy(d => d.DataTime).ToList();
        var series = new List<ChartSeries>();

        var tempPoints = sorted
            .Where(d => d.Temperature.HasValue)
            .Select(d => new ChartPoint(d.DataTime, (decimal)d.Temperature!.Value))
            .ToArray();
        if (tempPoints.Length > 0)
            series.Add(new ChartSeries { Name = "Temperature", Points = tempPoints });

        var humPoints = sorted
            .Where(d => d.Humidity.HasValue)
            .Select(d => new ChartPoint(d.DataTime, (decimal)d.Humidity!.Value))
            .ToArray();
        if (humPoints.Length > 0)
            series.Add(new ChartSeries { Name = "Humidity", Points = humPoints });

        var lightPoints = sorted
            .Where(d => d.Light.HasValue)
            .Select(d => new ChartPoint(d.DataTime, (decimal)d.Light!.Value))
            .ToArray();
        if (lightPoints.Length > 0)
            series.Add(new ChartSeries { Name = "Light", Points = lightPoints });

        var co2Points = sorted
            .Where(d => d.Co2.HasValue)
            .Select(d => new ChartPoint(d.DataTime, (decimal)d.Co2!.Value))
            .ToArray();
        if (co2Points.Length > 0)
            series.Add(new ChartSeries { Name = "CO2", Points = co2Points });

        return series;
    }

    private async Task OnTimeRangeChanged(long[] range)
    {
        _timeRange = range;
        await LoadProbeDataAsync();
    }

    private bool IsProbeConnected(int probeType)
    {
        if (_latestReadings.TryGetValue(probeType, out var reading) && reading is not null)
            return reading.Status != 1;
        return false;
    }

    private async Task ExportCsv(int probeType)
    {
        if (!_probeData.TryGetValue(probeType, out var data) || data.Count == 0)
            return;

        var sb = new StringBuilder();
        sb.AppendLine("DateTime,Temperature,Humidity,Light,CO2");
        foreach (var item in data.OrderBy(d => d.DataTime))
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds(item.DataTime).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            sb.AppendLine($"{time},{item.Temperature},{item.Humidity},{item.Light},{item.Co2}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var base64 = Convert.ToBase64String(bytes);
        var fileName = $"{_device?.DeviceName ?? "device"}_probe{probeType}_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
        var dataUrl = $"data:text/csv;base64,{base64}";

        var fileNameJs = JsonSerializer.Serialize(fileName);
        var dataUrlJs = JsonSerializer.Serialize(dataUrl);
        await JsRuntime.InvokeVoidAsync("eval",
            $"var a=document.createElement('a');a.href={dataUrlJs};a.download={fileNameJs};document.body.appendChild(a);a.click();document.body.removeChild(a);");
    }

    private static string GetProbeLabel(int probeType) => probeType switch
    {
        0 => "Built-in",
        1 => "External 1",
        2 => "External 2",
        _ => $"Probe {probeType}"
    };

    private static string GetAlarmPropertyName(int property) => property switch
    {
        1 => "Temperature",
        2 => "Humidity",
        3 => "Light",
        4 => "CO2",
        _ => $"Property {property}"
    };

    private static string GetAlarmTypeName(int type) => type switch
    {
        1 => "Low",
        2 => "High",
        _ => $"Type {type}"
    };

    private static string GetHandleStatusName(int status) => status switch
    {
        1 => "Processing",
        2 => "Processed",
        3 => "Ignored",
        _ => $"Status {status}"
    };

    private static ApexChartOptions<ChartPoint> ApexOptsFor(string name) => new()
    {
        Chart = new Chart { Background = "transparent", Toolbar = new Toolbar { Show = true }, Zoom = new Zoom { Enabled = true } },
        Stroke = new Stroke { Curve = Curve.Smooth, Width = 2 },
        Markers = new Markers { Size = 0 },
        Xaxis = new XAxis { Type = XAxisType.Datetime, Labels = new XAxisLabels { DatetimeUTC = false } },
        Yaxis = [new YAxis { Title = new AxisTitle { Text = name }, DecimalsInFloat = 1 }],
        Tooltip = new Tooltip { X = new TooltipX { Format = "dd MMM yyyy HH:mm" } },
        Legend = new Legend { Show = false }
    };

    private static string ApexColor(string name) => name switch
    {
        "Temperature" => "#F44336",
        "Humidity" => "#2196F3",
        "Light" => "#FFC107",
        "CO2" => "#4CAF50",
        _ => "#888"
    };

    public class ChartPoint
    {
        public DateTime Time { get; set; }
        public decimal Value { get; set; }

        public ChartPoint() { }
        public ChartPoint(long epochMs, decimal value)
        {
            Time = DateTimeOffset.FromUnixTimeMilliseconds(epochMs).LocalDateTime;
            Value = value;
        }
    }

    public class ChartSeries
    {
        public string Name { get; set; } = "";
        public ChartPoint[] Points { get; set; } = [];
    }
}
