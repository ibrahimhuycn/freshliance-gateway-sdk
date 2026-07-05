using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Response model containing device counts categorized by status for a group.
/// </summary>
public class DeviceGroupCountResponse
{
    /// <summary>
    /// The total number of devices in the group.
    /// </summary>
    [JsonPropertyName("deviceCount")] public int DeviceCount { get; set; }
    /// <summary>
    /// The number of devices currently online.
    /// </summary>
    [JsonPropertyName("onlineCount")] public int OnlineCount { get; set; }
    /// <summary>
    /// The number of devices currently offline.
    /// </summary>
    [JsonPropertyName("offlineCount")] public int OfflineCount { get; set; }
    /// <summary>
    /// The number of devices in an abnormal state.
    /// </summary>
    [JsonPropertyName("abnormalCount")] public int AbnormalCount { get; set; }
    /// <summary>
    /// The number of inactive devices.
    /// </summary>
    [JsonPropertyName("inactiveCount")] public int InactiveCount { get; set; }
    /// <summary>
    /// The number of devices currently in alarm.
    /// </summary>
    [JsonPropertyName("alarmCount")] public int AlarmCount { get; set; }
}
