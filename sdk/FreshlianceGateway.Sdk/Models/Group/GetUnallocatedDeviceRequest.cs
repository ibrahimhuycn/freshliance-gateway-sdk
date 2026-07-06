using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for retrieving devices not yet allocated to any group.
/// </summary>
public class GetUnallocatedDeviceRequest : IBizContent
{
    /// <summary>
    /// The page number for pagination. Default is 1.
    /// </summary>
    [JsonPropertyName("pageNum")] public int PageNum { get; set; } = 1;
    /// <summary>
    /// The number of items per page. Default is 10.
    /// </summary>
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 10;
    /// <summary>
    /// The group identifier context for the query.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
    /// <summary>
    /// Optional filter by device serial number.
    /// </summary>
    [JsonPropertyName("deviceSn")] public string? DeviceSn { get; set; }
}
