using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response containing counts of sub-devices grouped by their operational state.
/// </summary>
public class DeviceStateCountResponse
{
    /// <summary>Total number of sub-devices.</summary>
    [JsonPropertyName("deviceCount")] public int DeviceCount { get; set; }
    /// <summary>Number of sub-devices currently online.</summary>
    [JsonPropertyName("onlineCount")] public int OnlineCount { get; set; }
    /// <summary>Number of sub-devices currently offline.</summary>
    [JsonPropertyName("offlineCount")] public int OfflineCount { get; set; }
    /// <summary>Number of sub-devices in alarm state.</summary>
    [JsonPropertyName("alarmCount")] public int AlarmCount { get; set; }
    /// <summary>Number of sub-devices in abnormal state.</summary>
    [JsonPropertyName("abnormalCount")] public int AbnormalCount { get; set; }
}
