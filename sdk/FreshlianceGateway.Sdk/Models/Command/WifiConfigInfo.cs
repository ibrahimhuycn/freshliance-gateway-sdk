using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// WiFi and notification configuration for a device, including email, SMS, and voice alarm notification settings.
/// </summary>
public class WifiConfigInfo
{
    /// <summary>
    /// The user device identifier associated with this WiFi configuration.
    /// </summary>
    [JsonPropertyName("userDeviceId")] public int UserDeviceId { get; set; }
    /// <summary>
    /// Flag indicating whether email notifications are enabled.
    /// </summary>
    [JsonPropertyName("notifyEmailFlag")] public int? NotifyEmailFlag { get; set; }
    /// <summary>
    /// Maximum number of email notifications sent per day.
    /// </summary>
    [JsonPropertyName("notifyDayEmailCount")] public int? NotifyDayEmailCount { get; set; }
    /// <summary>
    /// Interval in minutes between email notifications.
    /// </summary>
    [JsonPropertyName("notifyEmailInterval")] public int? NotifyEmailInterval { get; set; }
    /// <summary>
    /// Maximum number of email notifications sent per alarm event.
    /// </summary>
    [JsonPropertyName("notifySingleEmailCount")] public int? NotifySingleEmailCount { get; set; }
    /// <summary>
    /// Email address to receive alarm notifications.
    /// </summary>
    [JsonPropertyName("notifyAlarmEmail")] public string? NotifyAlarmEmail { get; set; }
    /// <summary>
    /// Flag indicating whether SMS notifications are enabled.
    /// </summary>
    [JsonPropertyName("notifySmsFlag")] public int? NotifySmsFlag { get; set; }
    /// <summary>
    /// Maximum number of SMS notifications sent per day.
    /// </summary>
    [JsonPropertyName("notifyDaySmsCount")] public int? NotifyDaySmsCount { get; set; }
    /// <summary>
    /// Interval in minutes between SMS notifications.
    /// </summary>
    [JsonPropertyName("notifySmsInterval")] public int? NotifySmsInterval { get; set; }
    /// <summary>
    /// Maximum number of SMS notifications sent per alarm event.
    /// </summary>
    [JsonPropertyName("notifySingleSmsCount")] public int? NotifySingleSmsCount { get; set; }
    /// <summary>
    /// Phone number to receive SMS alarm notifications.
    /// </summary>
    [JsonPropertyName("notifyAlarmSms")] public string? NotifyAlarmSms { get; set; }
    /// <summary>
    /// Flag indicating whether voice call notifications are enabled.
    /// </summary>
    [JsonPropertyName("notifyVoiceFlag")] public int? NotifyVoiceFlag { get; set; }
    /// <summary>
    /// Maximum number of voice call notifications per day.
    /// </summary>
    [JsonPropertyName("notifyDayVoiceCount")] public int? NotifyDayVoiceCount { get; set; }
    /// <summary>
    /// Interval in minutes between voice call notifications.
    /// </summary>
    [JsonPropertyName("notifyVoiceInterval")] public int? NotifyVoiceInterval { get; set; }
    /// <summary>
    /// Maximum number of voice call notifications per alarm event.
    /// </summary>
    [JsonPropertyName("notifySingleVoiceCount")] public int? NotifySingleVoiceCount { get; set; }
    /// <summary>
    /// Phone number to receive voice call alarm notifications.
    /// </summary>
    [JsonPropertyName("notifyAlarmVoice")] public string? NotifyAlarmVoice { get; set; }
    /// <summary>
    /// Flag indicating whether time-based notification restrictions are enabled.
    /// </summary>
    [JsonPropertyName("notifyTimeFlag")] public int? NotifyTimeFlag { get; set; }
    /// <summary>
    /// Start time for the notification time window (HH:mm format).
    /// </summary>
    [JsonPropertyName("notifyStartTime")] public string? NotifyStartTime { get; set; }
    /// <summary>
    /// End time for the notification time window (HH:mm format).
    /// </summary>
    [JsonPropertyName("notifyEndTime")] public string? NotifyEndTime { get; set; }
    /// <summary>
    /// Specific dates for notifications (date format string).
    /// </summary>
    [JsonPropertyName("notifyDate")] public string? NotifyDate { get; set; }
}
