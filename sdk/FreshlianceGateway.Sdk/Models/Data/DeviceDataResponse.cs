using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Data;

/// <summary>
/// Response model containing a single data point from a device sensor reading.
/// </summary>
public class DeviceDataResponse
{
    /// <summary>
    /// The temperature value in degrees Celsius.
    /// </summary>
    [JsonPropertyName("temperature")] public double? Temperature { get; set; }
    /// <summary>
    /// The relative humidity percentage.
    /// </summary>
    [JsonPropertyName("humidity")] public double? Humidity { get; set; }
    /// <summary>
    /// The light intensity value.
    /// </summary>
    [JsonPropertyName("light")] public double? Light { get; set; }
    /// <summary>
    /// The CO2 concentration value.
    /// </summary>
    [JsonPropertyName("co2")] public double? Co2 { get; set; }
    /// <summary>
    /// The UTC timestamp of the data reading.
    /// </summary>
    [JsonPropertyName("dataTime")] public string DataTime { get; set; } = "";
    /// <summary>
    /// The probe type indicator (e.g., temperature, humidity).
    /// </summary>
    [JsonPropertyName("probeType")] public int ProbeType { get; set; }
    /// <summary>
    /// The device status code.
    /// </summary>
    [JsonPropertyName("status")] public int Status { get; set; }
}
