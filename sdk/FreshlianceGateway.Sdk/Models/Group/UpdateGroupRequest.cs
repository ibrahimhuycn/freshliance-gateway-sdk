using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for updating an existing device group.
/// </summary>
public class UpdateGroupRequest : IBizContent
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
