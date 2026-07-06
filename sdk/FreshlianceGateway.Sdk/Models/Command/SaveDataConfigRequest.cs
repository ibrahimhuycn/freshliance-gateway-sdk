using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Request DTO for saving a complete data logger configuration including command settings,
/// sensor alarms, WiFi notification settings, probe info, and collection/upload intervals.
/// </summary>
public class SaveDataConfigRequest : IBizContent
{
    /// <summary>
    /// The issued device command containing the record identifier.
    /// </summary>
    [JsonPropertyName("issuedDeviceCmd")] public IssuedDeviceCmd IssuedDeviceCmd { get; set; } = new();
    /// <summary>
    /// List of sensor alarm configurations for this data logger.
    /// </summary>
    [JsonPropertyName("sensorAlarmList")] public List<SensorAlarmItem>? SensorAlarmList { get; set; }
    /// <summary>
    /// WiFi and notification configuration information.
    /// </summary>
    [JsonPropertyName("wifiConfigInfo")] public WifiConfigInfo WifiConfigInfo { get; set; } = new();
    /// <summary>
    /// Device command configuration including time zone, buzzer status, and device name.
    /// </summary>
    [JsonPropertyName("issuedDeviceCmdConfig")] public IssuedDeviceCmdConfig IssuedDeviceCmdConfig { get; set; } = new();
    /// <summary>
    /// Delay in minutes before the device starts logging data.
    /// </summary>
    [JsonPropertyName("startDelay")] public int? StartDelay { get; set; }
    /// <summary>
    /// Data collection interval in minutes.
    /// </summary>
    [JsonPropertyName("collectInterval")] public int CollectInterval { get; set; }
    /// <summary>
    /// Data upload interval in minutes.
    /// </summary>
    [JsonPropertyName("uploadInterval")] public int UploadInterval { get; set; }
    /// <summary>
    /// List of probe configurations attached to this data logger.
    /// </summary>
    [JsonPropertyName("probeInfoList")] public List<ProbeInfoItem> ProbeInfoList { get; set; } = [];
}
