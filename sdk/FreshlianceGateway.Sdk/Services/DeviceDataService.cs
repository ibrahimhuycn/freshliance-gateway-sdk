using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Data;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Implements <see cref="IDeviceDataService"/>.</summary>
public class DeviceDataService : IDeviceDataService
{
    private readonly FreshlianceClient _client;

    /// <summary>Initializes a new instance of <see cref="DeviceDataService"/> with the given Freshliance client.</summary>
    public DeviceDataService(FreshlianceClient client) => _client = client;

    /// <summary>See <see cref="IDeviceDataService.GetDataPageAsync"/>.</summary>
    public Task<FreshlianceResponse<PageResult<DeviceDataResponse>>> GetDataPageAsync(GetDeviceDataRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<DeviceDataResponse>>("gw.deviceData.page", request, ct);

    /// <summary>See <see cref="IDeviceDataService.GetAlarmPageAsync"/>.</summary>
    /// <remarks>
    /// The gateway docs are self-contradictory here: the "Interface Method" heading lists
    /// <c>gw.deviceAlarmData.page</c>, but the request example uses <c>gw.deviceAlarm.page</c>.
    /// Verified against the live gateway: <c>gw.deviceAlarm.page</c> returns <c>code=0</c> (success),
    /// while <c>gw.deviceAlarmData.page</c> returns <c>40000 Parameter error</c>. Use the former.
    /// </remarks>
    public Task<FreshlianceResponse<PageResult<AlarmDataResponse>>> GetAlarmPageAsync(GetAlarmDataRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<AlarmDataResponse>>("gw.deviceAlarm.page", request, ct);
}
