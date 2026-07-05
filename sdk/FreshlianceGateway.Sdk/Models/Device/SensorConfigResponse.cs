using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response representing a sensor's configuration, including its thresholds, alarm status, and category assignment.
/// </summary>
public class SensorConfigResponse
{
    /// <summary>Unique identifier for the sensor configuration.</summary>
    [JsonPropertyName("configId")] public int ConfigId { get; set; }
    /// <summary>Type of probe sensor.</summary>
    [JsonPropertyName("probeType")] public int ProbeType { get; set; }
    /// <summary>Category identifier assigned to this sensor.</summary>
    [JsonPropertyName("categoryId")] public int? CategoryId { get; set; }
    /// <summary>Property code of the probe (e.g., internal/external).</summary>
    [JsonPropertyName("probeProperty")] public int ProbeProperty { get; set; }
    /// <summary>Current alarm status of the probe.</summary>
    [JsonPropertyName("probeAlarmStatus")] public int ProbeAlarmStatus { get; set; }
    /// <summary>Timestamp (epoch milliseconds) of the last probe alarm event.</summary>
    [JsonPropertyName("probeAlarmTime")] public long? ProbeAlarmTime { get; set; }
    /// <summary>Alarm rule identifier associated with the probe.</summary>
    [JsonPropertyName("probeAlarmId")] public int? ProbeAlarmId { get; set; }
    /// <summary>Identifier of the parent device or record.</summary>
    [JsonPropertyName("parentId")] public int ParentId { get; set; }
    /// <summary>Product code of the device model.</summary>
    [JsonPropertyName("productCode")] public string ProductCode { get; set; } = "";
    /// <summary>Configured high temperature alarm threshold.</summary>
    [JsonPropertyName("temHigh")] public double? TemHigh { get; set; }
    /// <summary>Configured low temperature alarm threshold.</summary>
    [JsonPropertyName("temLow")] public double? TemLow { get; set; }
    /// <summary>Configured high humidity alarm threshold.</summary>
    [JsonPropertyName("humHigh")] public double? HumHigh { get; set; }
    /// <summary>Configured low humidity alarm threshold.</summary>
    [JsonPropertyName("humLow")] public double? HumLow { get; set; }
    /// <summary>Configured high light alarm threshold in lux.</summary>
    [JsonPropertyName("lightHigh")] public double? LightHigh { get; set; }
    /// <summary>Configured low light alarm threshold in lux.</summary>
    [JsonPropertyName("lightLow")] public double? LightLow { get; set; }
    /// <summary>Configured high CO2 alarm threshold in ppm.</summary>
    [JsonPropertyName("co2High")] public double? Co2High { get; set; }
    /// <summary>Configured low CO2 alarm threshold in ppm.</summary>
    [JsonPropertyName("co2Low")] public double? Co2Low { get; set; }
}
