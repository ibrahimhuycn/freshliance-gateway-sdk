using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Request DTO for updating device parameters such as buzzer status and temperature unit.
/// </summary>
public class UpdateParameterRequest : IBizContent
{
    /// <summary>
    /// The unique record identifier of the device configuration.
    /// </summary>
    [JsonPropertyName("recordId")] public int RecordId { get; set; }
    /// <summary>
    /// Buzzer status (0 = off, 1 = on).
    /// </summary>
    [JsonPropertyName("buzzerStatus")] public int BuzzerStatus { get; set; }
    /// <summary>
    /// Temperature unit (0 = Celsius, 1 = Fahrenheit).
    /// </summary>
    [JsonPropertyName("temperatureUnit")] public int TemperatureUnit { get; set; }
}
