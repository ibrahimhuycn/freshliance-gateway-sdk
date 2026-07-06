using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Data;

/// <summary>
/// Request model for retrieving device data.
/// </summary>
public class GetDeviceDataRequest : IBizContent
{
    /// <summary>
    /// The record identifier.
    /// </summary>
    [JsonPropertyName("recordId")] public int RecordId { get; set; }
    /// <summary>
    /// The probe type (e.g., temperature, humidity).
    /// </summary>
    [JsonPropertyName("probeType")] public int ProbeType { get; set; }
    /// <summary>
    /// The page number for pagination. Default is 1.
    /// </summary>
    [JsonPropertyName("pageNum")] public int PageNum { get; set; } = 1;
    /// <summary>
    /// The number of items per page. Default is 10.
    /// </summary>
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 10;
    /// <summary>
    /// Optional time range filter for data timestamps (start and end).
    /// </summary>
    [JsonPropertyName("dataTime")] public long[]? DataTime { get; set; }
}
