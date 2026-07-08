using Microsoft.Extensions.Options;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Services;

namespace Freshliance.Dashboard.Services;

public sealed class PollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PollingService> _logger;
    private readonly int _intervalSeconds;
    private readonly bool _enabled;
    private int _consecutiveFailures;

    public event Action<DashboardSnapshot>? SnapshotUpdated;

    public DashboardSnapshot? CurrentSnapshot { get; private set; }

    public PollingService(
        IServiceScopeFactory scopeFactory,
        IOptions<PollingOptions> options,
        ILogger<PollingService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _intervalSeconds = options.Value.IntervalSeconds;
        _enabled = options.Value.Enabled;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_enabled)
        {
            _logger.LogInformation("Polling disabled — service will not run");
            return;
        }

        _logger.LogInformation("Polling started — interval {Interval}s", _intervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
                await PollAsync();
                _consecutiveFailures = 0;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _consecutiveFailures++;
                _logger.LogWarning(ex, "Poll cycle failed ({FailureCount} consecutive)", _consecutiveFailures);

                if (_consecutiveFailures >= 5)
                {
                    _logger.LogError("Poll cycle failed 5 consecutive times — broadcasting empty snapshot");
                    BroadcastEmptySnapshot();
                }
            }
        }

        _logger.LogInformation("Polling stopped");
    }

    private async Task PollAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();
        var dataService = scope.ServiceProvider.GetRequiredService<IDeviceDataService>();

        var deviceResponse = await deviceService.GetPageAsync(new GetDevicePageRequest { PageSize = 50, PageNum = 1 });
        deviceResponse.EnsureSuccess();
        var devices = deviceResponse.Data?.Rows ?? [];

        var totalDevices = deviceResponse.Data?.Total ?? devices.Count;
        var onlineCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 1);
        var offlineCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 2);
        var abnormalCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 3);
        var inactiveCount = devices.Count(d => d.DeviceInfo.DeviceStatus == 0);
        var alarmCount = devices.Count(d => d.DeviceInfo.AlarmStatus == 2);

        var snapshot = new DashboardSnapshot(
            totalDevices,
            onlineCount,
            offlineCount,
            alarmCount,
            abnormalCount,
            inactiveCount,
            DateTimeOffset.UtcNow,
            []
        );

        CurrentSnapshot = snapshot;
        SnapshotUpdated?.Invoke(snapshot);

        _logger.LogDebug(
            "Poll complete — {Total} devices, {Online} online, {Offline} offline, {Alarm} in alarm",
            totalDevices, onlineCount, offlineCount, alarmCount);
    }

    private void BroadcastEmptySnapshot()
    {
        var snapshot = new DashboardSnapshot(0, 0, 0, 0, 0, 0, DateTimeOffset.UtcNow, []);
        CurrentSnapshot = snapshot;
        SnapshotUpdated?.Invoke(snapshot);
    }
}
