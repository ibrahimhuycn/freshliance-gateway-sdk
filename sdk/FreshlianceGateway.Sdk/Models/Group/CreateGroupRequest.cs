using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for creating a new device group.
/// </summary>
public class CreateGroupRequest : IBizContent
{
    /// <summary>
    /// The parent group identifier. Use 0 to create a root group.
    /// </summary>
    [JsonPropertyName("parentId")] public int ParentId { get; set; }
    /// <summary>
    /// The display name of the new group.
    /// </summary>
    [JsonPropertyName("groupName")] public string GroupName { get; set; } = "";
}
