using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Group;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Provides operations for managing device-to-group assignments in Freshliance Cloud.</summary>
public interface IGroupDeviceService
{
    /// <summary>Retrieves a paginated list of devices not yet allocated to any group.</summary>
    Task<FreshlianceResponse<PageResult<UnallocatedDeviceResponse>>> GetUnallocatedPageAsync(GetUnallocatedDeviceRequest request, CancellationToken ct = default);
    /// <summary>Retrieves a paginated list of devices currently allocated to a group.</summary>
    Task<FreshlianceResponse<PageResult<AllocatedDeviceResponse>>> GetAllocatedPageAsync(GetAllocatedDeviceRequest request, CancellationToken ct = default);
    /// <summary>Unbinds (removes) a device from its assigned group.</summary>
    Task<FreshlianceResponse<bool>> UnbindAsync(UnbindDeviceRequest request, CancellationToken ct = default);
    /// <summary>Binds (assigns) a device to a group.</summary>
    Task<FreshlianceResponse<bool>> BindAsync(BindDeviceRequest request, CancellationToken ct = default);
}
