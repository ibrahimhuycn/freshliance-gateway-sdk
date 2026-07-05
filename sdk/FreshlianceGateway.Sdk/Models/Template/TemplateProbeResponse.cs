using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Response DTO representing a probe configuration within a template, including high/low threshold limits
/// for various sensor types and associated alarm configurations.
/// </summary>
public class TemplateProbeResponse
{
    /// <summary>
    /// The configuration identifier for this probe.
    /// </summary>
    [JsonPropertyName("configId")] public int ConfigId { get; set; }
    /// <summary>
    /// The type of probe (e.g., temperature, humidity, light, CO2).
    /// </summary>
    [JsonPropertyName("probeType")] public int ProbeType { get; set; }
    /// <summary>
    /// Upper temperature limit.
    /// </summary>
    [JsonPropertyName("temHigh")] public double? TemHigh { get; set; }
    /// <summary>
    /// Lower temperature limit.
    /// </summary>
    [JsonPropertyName("temLow")] public double? TemLow { get; set; }
    /// <summary>
    /// Upper humidity limit.
    /// </summary>
    [JsonPropertyName("humHigh")] public double? HumHigh { get; set; }
    /// <summary>
    /// Lower humidity limit.
    /// </summary>
    [JsonPropertyName("humLow")] public double? HumLow { get; set; }
    /// <summary>
    /// Upper light intensity limit.
    /// </summary>
    [JsonPropertyName("lightHigh")] public double? LightHigh { get; set; }
    /// <summary>
    /// Lower light intensity limit.
    /// </summary>
    [JsonPropertyName("lightLow")] public double? LightLow { get; set; }
    /// <summary>
    /// Upper CO2 concentration limit.
    /// </summary>
    [JsonPropertyName("co2High")] public double? Co2High { get; set; }
    /// <summary>
    /// Lower CO2 concentration limit.
    /// </summary>
    [JsonPropertyName("co2Low")] public double? Co2Low { get; set; }
    /// <summary>
    /// List of alarm configurations associated with this probe.
    /// </summary>
    [JsonPropertyName("sensorAlarmList")] public List<TemplateAlarmResponse>? SensorAlarmList { get; set; }
}
