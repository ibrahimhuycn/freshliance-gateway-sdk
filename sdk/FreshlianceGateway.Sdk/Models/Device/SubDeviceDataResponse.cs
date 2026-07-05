using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response containing the latest sensor data readings from a sub-device.
/// </summary>
public class SubDeviceDataResponse
{
    /// <summary>Current temperature reading in the configured unit.</summary>
    [JsonPropertyName("temperature")] public double? Temperature { get; set; }
    /// <summary>Current humidity reading as a percentage.</summary>
    [JsonPropertyName("humidity")] public double? Humidity { get; set; }
    /// <summary>Current ambient light reading in lux.</summary>
    [JsonPropertyName("light")] public double? Light { get; set; }
    /// <summary>Current CO2 concentration reading in ppm.</summary>
    [JsonPropertyName("co2")] public double? Co2 { get; set; }
    /// <summary>Timestamp (epoch milliseconds) when the data was recorded.</summary>
    [JsonPropertyName("dataTime")] public long? DataTime { get; set; }
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
    /// <summary>Type of probe sensor.</summary>
    [JsonPropertyName("probeType")] public int ProbeType { get; set; }
    /// <summary>Property code of the probe (e.g., internal/external).</summary>
    [JsonPropertyName("probeProperty")] public int ProbeProperty { get; set; }
    /// <summary>Current alarm status of the probe.</summary>
    [JsonPropertyName("probeAlarmStatus")] public int ProbeAlarmStatus { get; set; }
    /// <summary>Timestamp (epoch milliseconds) of the last probe alarm event.</summary>
    [JsonPropertyName("probeAlarmTime")] public long? ProbeAlarmTime { get; set; }
    /// <summary>Operational/connection status of the sub-device.</summary>
    [JsonPropertyName("status")] public int Status { get; set; }
}
