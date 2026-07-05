using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for updating an existing device group.
/// </summary>
public class UpdateGroupRequest
{
    /// <summary>
    /// The unique identifier of the group to update.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
    /// <summary>
    /// The new display name for the group.
    /// </summary>
    [JsonPropertyName("groupName")] public string GroupName { get; set; } = "";
}
