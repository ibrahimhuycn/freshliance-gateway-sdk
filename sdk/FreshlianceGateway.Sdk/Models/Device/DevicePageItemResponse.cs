using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response item for a single device in a paged device list, including device info, sub-device data, and state counts.
/// </summary>
public class DevicePageItemResponse
{
    /// <summary>Core device information.</summary>
    [JsonPropertyName("deviceInfo")] public DeviceInfoResponse DeviceInfo { get; set; } = new();
    /// <summary>ID of the parent user who owns this device.</summary>
    [JsonPropertyName("userParentId")] public int? UserParentId { get; set; }
    /// <summary>Latest data readings from sub-devices attached to this device.</summary>
    [JsonPropertyName("subDeviceLastDataList")] public List<SubDeviceDataResponse> SubDeviceLastDataList { get; set; } = [];
    /// <summary>Counts of sub-devices in each operational state.</summary>
    [JsonPropertyName("deviceDeviceStateCount")] public DeviceStateCountResponse? DeviceDeviceStateCount { get; set; }
}
