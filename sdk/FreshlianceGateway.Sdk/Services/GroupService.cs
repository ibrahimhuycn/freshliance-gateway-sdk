using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Group;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Implements <see cref="IGroupService"/>.</summary>
public class GroupService : IGroupService
{
    private readonly FreshlianceClient _client;

    /// <summary>Initializes a new instance of <see cref="GroupService"/> with the given Freshliance client.</summary>
    public GroupService(FreshlianceClient client) => _client = client;

    /// <summary>See <see cref="IGroupService.GetTreeAsync"/>.</summary>
    public Task<FreshlianceResponse<List<GroupTreeNodeResponse>>> GetTreeAsync(CancellationToken ct = default)
        => _client.PostAsync<List<GroupTreeNodeResponse>>("gw.deviceGroup.treeList", null, ct);

    /// <summary>See <see cref="IGroupService.GetListAsync"/>.</summary>
    public Task<FreshlianceResponse<List<GroupListItemResponse>>> GetListAsync(GetGroupListRequest request, CancellationToken ct = default)
        => _client.PostAsync<List<GroupListItemResponse>>("gw.deviceGroup.list", request, ct);

    /// <summary>See <see cref="IGroupService.CreateAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> CreateAsync(CreateGroupRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.deviceGroup.create", request, ct);

    /// <summary>See <see cref="IGroupService.UpdateAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> UpdateAsync(UpdateGroupRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.deviceGroup.update", request, ct);

    /// <summary>See <see cref="IGroupService.DeleteAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> DeleteAsync(DeleteGroupRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.deviceGroup.delete", request, ct);
}
