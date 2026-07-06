using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for retrieving a list of groups under a parent group.
/// </summary>
public class GetGroupListRequest : IBizContent
{
    /// <summary>
    /// The parent group identifier. Use 0 for root-level groups.
    /// </summary>
    [JsonPropertyName("parentId")] public int ParentId { get; set; }
    /// <summary>
    /// Optional keyword to filter groups by name.
    /// </summary>
    [JsonPropertyName("keyword")] public string? Keyword { get; set; }
}
