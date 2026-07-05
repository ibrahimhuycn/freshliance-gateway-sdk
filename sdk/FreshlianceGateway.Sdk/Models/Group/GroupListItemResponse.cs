using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Response model representing a flat list item in a group listing.
/// </summary>
public class GroupListItemResponse
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
    /// The number of devices in this group.
    /// </summary>
    [JsonPropertyName("deviceCount")] public int DeviceCount { get; set; }
}
