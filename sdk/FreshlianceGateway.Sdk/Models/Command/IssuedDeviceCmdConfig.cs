using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Device command configuration including temperature unit, time zone, buzzer status, and device name.
/// </summary>
public class IssuedDeviceCmdConfig
{
    /// <summary>
    /// Temperature unit (0 = Celsius, 1 = Fahrenheit).
    /// </summary>
    [JsonPropertyName("temperatureUnit")] public int TemperatureUnit { get; set; }
    /// <summary>
    /// Time zone identifier for the device.
    /// </summary>
    [JsonPropertyName("timeZone")] public string TimeZone { get; set; } = "";
    /// <summary>
    /// Buzzer status (0 = off, 1 = on).
    /// </summary>
    [JsonPropertyName("buzzerStatus")] public int BuzzerStatus { get; set; }
    /// <summary>
    /// The time at which the configuration was applied.
    /// </summary>
    [JsonPropertyName("configTime")] public string? ConfigTime { get; set; }
    /// <summary>
    /// User-friendly name for the device.
    /// </summary>
    [JsonPropertyName("deviceName")] public string? DeviceName { get; set; }
}
