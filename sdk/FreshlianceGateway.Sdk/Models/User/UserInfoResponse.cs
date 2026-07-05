using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.User;

/// <summary>
/// Response containing the current user's profile and notification settings.
/// </summary>
public class UserInfoResponse
{
    /// <summary>User's email address.</summary>
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    /// <summary>User's time zone identifier (e.g., "Asia/Shanghai").</summary>
    [JsonPropertyName("timeZone")] public string TimeZone { get; set; } = "";
    /// <summary>User's preferred language code.</summary>
    [JsonPropertyName("language")] public int Language { get; set; }
    /// <summary>User's preferred date format code.</summary>
    [JsonPropertyName("dateFormat")] public int DateFormat { get; set; }
    /// <summary>User's preferred temperature unit (e.g., 0=Celsius, 1=Fahrenheit).</summary>
    [JsonPropertyName("temperatureUnit")] public int TemperatureUnit { get; set; }
    /// <summary>Number of China SMS notifications allowed.</summary>
    [JsonPropertyName("chnSmsNum")] public int ChnSmsNum { get; set; }
    /// <summary>Number of international SMS notifications allowed.</summary>
    [JsonPropertyName("intSmsNum")] public int IntSmsNum { get; set; }
    /// <summary>Number of China voice call notifications allowed.</summary>
    [JsonPropertyName("chnVoiceNum")] public int ChnVoiceNum { get; set; }
    /// <summary>Number of international voice call notifications allowed.</summary>
    [JsonPropertyName("intVoiceNum")] public int IntVoiceNum { get; set; }
    /// <summary>User's display nickname.</summary>
    [JsonPropertyName("nickName")] public string? Nickname { get; set; }
}
