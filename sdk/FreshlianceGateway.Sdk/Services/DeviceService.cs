using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Device;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Implements <see cref="IDeviceService"/>.</summary>
public class DeviceService : IDeviceService
{
    private readonly FreshlianceClient _client;

    /// <summary>Initializes a new instance of <see cref="DeviceService"/> with the given Freshliance client.</summary>
    public DeviceService(FreshlianceClient client) => _client = client;

    /// <summary>See <see cref="IDeviceService.GetCategoriesAsync"/>.</summary>
    public Task<FreshlianceResponse<List<CategoryResponse>>> GetCategoriesAsync(CancellationToken ct = default)
        => _client.PostAsync<List<CategoryResponse>>("gw.device.category", null, ct);

    /// <summary>See <see cref="IDeviceService.GetPageAsync"/>.</summary>
    public Task<FreshlianceResponse<PageResult<DevicePageItemResponse>>> GetPageAsync(GetDevicePageRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<DevicePageItemResponse>>("gw.deviceInfo.page", request, ct);

    /// <summary>See <see cref="IDeviceService.GetRecordPageAsync"/>.</summary>
    public Task<FreshlianceResponse<PageResult<RecordPageItemResponse>>> GetRecordPageAsync(GetRecordPageRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<RecordPageItemResponse>>("gw.deviceInfo.recordPage", request, ct);

    /// <summary>See <see cref="IDeviceService.GetSubDevicePageAsync"/>.</summary>
    public Task<FreshlianceResponse<PageResult<SubDevicePageItemResponse>>> GetSubDevicePageAsync(GetSubDevicePageRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<SubDevicePageItemResponse>>("gw.deviceInfo.subPage", request, ct);
}
