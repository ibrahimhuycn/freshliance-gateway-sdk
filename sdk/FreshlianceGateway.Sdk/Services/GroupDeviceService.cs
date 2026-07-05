using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Group;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Implements <see cref="IGroupDeviceService"/>.</summary>
public class GroupDeviceService : IGroupDeviceService
{
    private readonly FreshlianceClient _client;

    /// <summary>Initializes a new instance of <see cref="GroupDeviceService"/> with the given Freshliance client.</summary>
    public GroupDeviceService(FreshlianceClient client) => _client = client;

    /// <summary>See <see cref="IGroupDeviceService.GetUnallocatedPageAsync"/>.</summary>
    public Task<FreshlianceResponse<PageResult<UnallocatedDeviceResponse>>> GetUnallocatedPageAsync(GetUnallocatedDeviceRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<UnallocatedDeviceResponse>>("gw.groupDevice.pageUnAllocatedDevice", request, ct);

    /// <summary>See <see cref="IGroupDeviceService.GetAllocatedPageAsync"/>.</summary>
    public Task<FreshlianceResponse<PageResult<AllocatedDeviceResponse>>> GetAllocatedPageAsync(GetAllocatedDeviceRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<AllocatedDeviceResponse>>("gw.groupDevice.pageAllocatedDevice", request, ct);

    /// <summary>See <see cref="IGroupDeviceService.UnbindAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> UnbindAsync(UnbindDeviceRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.groupDevice.unbindDevice", request, ct);

    /// <summary>See <see cref="IGroupDeviceService.BindAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> BindAsync(BindDeviceRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.groupDevice.bindDevice", request, ct);
}
