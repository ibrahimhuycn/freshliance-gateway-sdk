using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Represents a sensor alarm configuration item with thresholds, alarm type, and notification settings.
/// </summary>
public class SensorAlarmItem
{
    /// <summary>
    /// The alarm zone identifier (e.g., "high", "low").
    /// </summary>
    [JsonPropertyName("alarmZone")] public string? AlarmZone { get; set; }
    /// <summary>
    /// The alarm property type (e.g., temperature, humidity).
    /// </summary>
    [JsonPropertyName("alarmProperty")] public int? AlarmProperty { get; set; }
    /// <summary>
    /// The alarm type (e.g., upper limit, lower limit).
    /// </summary>
    [JsonPropertyName("alarmType")] public int? AlarmType { get; set; }
    /// <summary>
    /// The probe type associated with this alarm.
    /// </summary>
    [JsonPropertyName("probeType")] public int? ProbeType { get; set; }
    /// <summary>
    /// The alarm notification method.
    /// </summary>
    [JsonPropertyName("alarmWay")] public int? AlarmWay { get; set; }
    /// <summary>
    /// Delay in minutes before the alarm triggers after threshold breach.
    /// </summary>
    [JsonPropertyName("alarmDelay")] public int? AlarmDelay { get; set; }
    /// <summary>
    /// The threshold value that triggers the alarm.
    /// </summary>
    [JsonPropertyName("alarmThreshold")] public double? AlarmThreshold { get; set; }
}
