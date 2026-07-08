using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Data;

/// <summary>
/// Request model for retrieving alarm data with optional filters.
/// </summary>
public class GetAlarmDataRequest : IBizContent
{
    /// <summary>
    /// The record identifier.
    /// </summary>
    [JsonPropertyName("recordId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? RecordId { get; set; }
    /// <summary>
    /// The alarm property (e.g., temperature threshold, humidity threshold).
    /// </summary>
    [JsonPropertyName("alarmProperty")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AlarmProperty { get; set; }
    /// <summary>
    /// Optional probe type filter.
    /// </summary>
    [JsonPropertyName("probeType")] public int? ProbeType { get; set; }
    /// <summary>
    /// Optional alarm type filter.
    /// </summary>
    [JsonPropertyName("alarmType")] public int? AlarmType { get; set; }
    /// <summary>
    /// Optional handle status filter.
    /// </summary>
    [JsonPropertyName("handleStatus")] public int? HandleStatus { get; set; }
    /// <summary>
    /// Optional start time filter for the alarm range (Unix timestamp).
    /// </summary>
    [JsonPropertyName("alarmStartTime")] public long? AlarmStartTime { get; set; }
    /// <summary>
    /// Optional end time filter for the alarm range (Unix timestamp).
    /// </summary>
    [JsonPropertyName("alarmEndTime")] public long? AlarmEndTime { get; set; }
    /// <summary>
    /// The page number for pagination. Default is 1.
    /// </summary>
    [JsonPropertyName("pageNum")] public int PageNum { get; set; } = 1;
    /// <summary>
    /// The number of items per page. Default is 10.
    /// </summary>
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 10;
}
