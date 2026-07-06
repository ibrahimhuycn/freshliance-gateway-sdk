using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.User;

/// <summary>
/// Request payload for updating the current user's profile settings.
/// Only supplied fields will be updated.
/// </summary>
public class UpdateUserInfoRequest : IBizContent
{
    /// <summary>User's time zone identifier (e.g., "America/New_York").</summary>
    [JsonPropertyName("timeZone")] public string? TimeZone { get; set; }
    /// <summary>User's preferred language code.</summary>
    [JsonPropertyName("language")] public int? Language { get; set; }
    /// <summary>User's preferred date format code.</summary>
    [JsonPropertyName("dateFormat")] public int? DateFormat { get; set; }
    /// <summary>User's preferred temperature unit (e.g., 0=Celsius, 1=Fahrenheit).</summary>
    [JsonPropertyName("temperatureUnit")] public int? TemperatureUnit { get; set; }
    /// <summary>User's display nickname.</summary>
    [JsonPropertyName("nickname")] public string? Nickname { get; set; }
}
