using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response item for a single sub-device in a paged sub-device list, including its info and latest sensor data.
/// </summary>
public class SubDevicePageItemResponse
{
    /// <summary>Core sub-device information.</summary>
    [JsonPropertyName("subDeviceInfo")] public SubDeviceInfoResponse? SubDeviceInfo { get; set; }
    /// <summary>Latest sensor data readings from the sub-device.</summary>
    [JsonPropertyName("subDeviceLastDataList")] public List<SubDeviceDataResponse>? SubDeviceLastDataList { get; set; }
}
