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
    public Task<FreshlianceResponse<PageResult<AlarmDataResponse>>> GetAlarmPageAsync(GetAlarmDataRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<AlarmDataResponse>>("gw.deviceAlarmData.page", request, ct);
}
