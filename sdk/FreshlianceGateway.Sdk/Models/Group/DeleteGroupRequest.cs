using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for deleting a device group.
/// </summary>
public class DeleteGroupRequest
{
    /// <summary>
    /// The unique identifier of the group to delete.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
}
