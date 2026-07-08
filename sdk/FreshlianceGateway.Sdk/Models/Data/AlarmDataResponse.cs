using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Data;

/// <summary>
/// Response model containing a single alarm record.
/// </summary>
public class AlarmDataResponse
{
    /// <summary>
    /// The unique identifier of the device alarm.
    /// </summary>
    [JsonPropertyName("deviceAlarmId")] public int DeviceAlarmId { get; set; }
    /// <summary>
    /// The record identifier associated with this alarm.
    /// </summary>
    [JsonPropertyName("recordId")] public int RecordId { get; set; }
    /// <summary>
    /// The probe type that triggered the alarm.
    /// </summary>
    [JsonPropertyName("probeType")] public int ProbeType { get; set; }
    /// <summary>
    /// The device identifier.
    /// </summary>
    [JsonPropertyName("deviceId")] public int DeviceId { get; set; }
    /// <summary>
    /// The parent group or device identifier.
    /// </summary>
    [JsonPropertyName("parentId")] public int ParentId { get; set; }
    /// <summary>
    /// The associated data point identifier.
    /// </summary>
    [JsonPropertyName("dataId")] public int DataId { get; set; }
    /// <summary>
    /// The name of the alarm zone.
    /// </summary>
    [JsonPropertyName("alarmZone")] public string AlarmZone { get; set; } = "";
    /// <summary>
    /// The alarm property (e.g., temperature threshold, humidity threshold).
    /// </summary>
    [JsonPropertyName("alarmProperty")] public int AlarmProperty { get; set; }
    /// <summary>
    /// The type of alarm (e.g., upper limit, lower limit).
    /// </summary>
    [JsonPropertyName("alarmType")] public int AlarmType { get; set; }
    /// <summary>
    /// The alarm notification method.
    /// </summary>
    [JsonPropertyName("alarmWay")] public int AlarmWay { get; set; }
    /// <summary>
    /// The alarm delay in seconds before triggering.
    /// </summary>
    [JsonPropertyName("alarmDelay")] public int AlarmDelay { get; set; }
    /// <summary>
    /// The threshold value that triggered the alarm.
    /// </summary>
    [JsonPropertyName("alarmThreshold")] public double AlarmThreshold { get; set; }
    /// <summary>
    /// The actual property value at the time of the alarm.
    /// </summary>
    [JsonPropertyName("propertyValue")] public int PropertyValue { get; set; }
    /// <summary>
    /// The UTC timestamp when the alarm occurred.
    /// </summary>
    [JsonPropertyName("alarmTime")] public long AlarmTime { get; set; }
    /// <summary>
    /// The name of the director or supervisor assigned.
    /// </summary>
    [JsonPropertyName("directorName")] public string? DirectorName { get; set; }
    /// <summary>
    /// The identifier of the handler who processed the alarm.
    /// </summary>
    [JsonPropertyName("handlerId")] public int? HandlerId { get; set; }
    /// <summary>
    /// The handling status code.
    /// </summary>
    [JsonPropertyName("handleStatus")] public int HandleStatus { get; set; }
    /// <summary>
    /// The Unix timestamp when the alarm was handled.
    /// </summary>
    [JsonPropertyName("handleTime")] public long? HandleTime { get; set; }
    /// <summary>
    /// The result or description of how the alarm was handled.
    /// </summary>
    [JsonPropertyName("handleResult")] public string? HandleResult { get; set; }
    /// <summary>
    /// The UTC timestamp when the record was created.
    /// </summary>
    [JsonPropertyName("createTime")] public long CreateTime { get; set; }
    /// <summary>
    /// The UTC timestamp when the record was last updated.
    /// </summary>
    [JsonPropertyName("updateTime")] public long UpdateTime { get; set; }
    /// <summary>
    /// Indicates whether the alarm record has been soft-deleted.
    /// </summary>
    [JsonPropertyName("deleted")] public bool Deleted { get; set; }
}
