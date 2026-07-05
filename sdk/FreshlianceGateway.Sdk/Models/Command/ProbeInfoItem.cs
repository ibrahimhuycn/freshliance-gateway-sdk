using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Represents a probe configuration item specifying the probe type and its property.
/// </summary>
public class ProbeInfoItem
{
    /// <summary>
    /// The type of probe (e.g., temperature, humidity, light, CO2).
    /// </summary>
    [JsonPropertyName("probeType")] public int ProbeType { get; set; }
    /// <summary>
    /// The property measured by the probe.
    /// </summary>
    [JsonPropertyName("probeProperty")] public int ProbeProperty { get; set; }
}
