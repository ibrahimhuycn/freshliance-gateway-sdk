using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Models.Device;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Response model representing a device allocated to a group.
/// </summary>
public class AllocatedDeviceResponse
{
    /// <summary>
    /// The user-device association identifier.
    /// </summary>
    [JsonPropertyName("userDeviceId")] public int UserDeviceId { get; set; }
    /// <summary>
    /// The device serial number.
    /// </summary>
    [JsonPropertyName("deviceSn")] public string DeviceSn { get; set; } = "";
    /// <summary>
    /// The display name of the device.
    /// </summary>
    [JsonPropertyName("deviceName")] public string DeviceName { get; set; } = "";
    /// <summary>
    /// The parent group identifier.
    /// </summary>
    [JsonPropertyName("parentId")] public int ParentId { get; set; }
    /// <summary>
    /// The product type code.
    /// </summary>
    [JsonPropertyName("productType")] public int ProductType { get; set; }
    /// <summary>
    /// The product model name.
    /// </summary>
    [JsonPropertyName("productModel")] public string? ProductModel { get; set; }
    /// <summary>
    /// The device code.
    /// </summary>
    [JsonPropertyName("deviceCode")] public string? DeviceCode { get; set; }
    /// <summary>
    /// The current device status code.
    /// </summary>
    [JsonPropertyName("deviceStatus")] public int DeviceStatus { get; set; }
    /// <summary>
    /// The UTC timestamp when the device was created.
    /// </summary>
    [JsonPropertyName("createTime")] public string? CreateTime { get; set; }
    /// <summary>
    /// The number of sub-devices attached to this device.
    /// </summary>
    [JsonPropertyName("subDeviceCount")] public int SubDeviceCount { get; set; }
    /// <summary>
    /// The status count breakdown of the sub-devices.
    /// </summary>
    [JsonPropertyName("subDeviceStatusCount")] public DeviceStateCountResponse? SubDeviceStatusCount { get; set; }
}
