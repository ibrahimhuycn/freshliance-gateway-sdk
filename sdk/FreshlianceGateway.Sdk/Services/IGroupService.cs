using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Group;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Provides operations for managing device groups in Freshliance Cloud.</summary>
public interface IGroupService
{
    /// <summary>Retrieves the full device group tree structure from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<List<GroupTreeNodeResponse>>> GetTreeAsync(CancellationToken ct = default);
    /// <summary>Retrieves a filtered list of device groups from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<List<GroupListItemResponse>>> GetListAsync(GetGroupListRequest request, CancellationToken ct = default);
    /// <summary>Creates a new device group in Freshliance Cloud.</summary>
    Task<FreshlianceResponse<bool>> CreateAsync(CreateGroupRequest request, CancellationToken ct = default);
    /// <summary>Updates an existing device group in Freshliance Cloud.</summary>
    Task<FreshlianceResponse<bool>> UpdateAsync(UpdateGroupRequest request, CancellationToken ct = default);
    /// <summary>Deletes a device group from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<bool>> DeleteAsync(DeleteGroupRequest request, CancellationToken ct = default);
}
