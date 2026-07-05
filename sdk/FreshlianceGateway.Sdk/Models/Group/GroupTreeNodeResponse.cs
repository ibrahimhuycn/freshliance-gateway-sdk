using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Response model representing a node in the group tree hierarchy.
/// </summary>
public class GroupTreeNodeResponse
{
    /// <summary>
    /// The unique identifier of the group.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
    /// <summary>
    /// The parent group identifier. 0 indicates a root group.
    /// </summary>
    [JsonPropertyName("parentId")] public int ParentId { get; set; }
    /// <summary>
    /// The display name of the group.
    /// </summary>
    [JsonPropertyName("groupName")] public string GroupName { get; set; } = "";
    /// <summary>
    /// The device count summary for this group.
    /// </summary>
    [JsonPropertyName("deviceGroupCount")] public DeviceGroupCountResponse? DeviceGroupCount { get; set; }
    /// <summary>
    /// The list of child group nodes.
    /// </summary>
    [JsonPropertyName("subDeviceGroupList")] public List<GroupTreeNodeResponse>? SubDeviceGroupList { get; set; }
}
