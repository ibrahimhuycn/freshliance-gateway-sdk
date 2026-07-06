using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for deleting a device group.
/// </summary>
public class DeleteGroupRequest : IBizContent
{
    /// <summary>
    /// The unique identifier of the group to delete.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
}
