using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response containing detailed information about a device.
/// </summary>
public class DeviceInfoResponse
{
    /// <summary>Unique identifier of the user-device association.</summary>
    [JsonPropertyName("userDeviceId")] public int UserDeviceId { get; set; }
    /// <summary>Product code of the device model.</summary>
    [JsonPropertyName("productCode")] public string ProductCode { get; set; } = "";
    /// <summary>Serial number of the device.</summary>
    [JsonPropertyName("deviceSn")] public string DeviceSn { get; set; } = "";
    /// <summary>Device communication code (e.g., IMEI or unique identifier).</summary>
    [JsonPropertyName("deviceCode")] public string DeviceCode { get; set; } = "";
    /// <summary>User-assigned name for the device.</summary>
    [JsonPropertyName("deviceName")] public string DeviceName { get; set; } = "";
    /// <summary>Internal device identifier.</summary>
    [JsonPropertyName("deviceId")] public int DeviceId { get; set; }
    /// <summary>Current record identifier associated with the device.</summary>
    [JsonPropertyName("recordId")] public int RecordId { get; set; }
    /// <summary>Product type code.</summary>
    [JsonPropertyName("productType")] public int ProductType { get; set; }
    /// <summary>Model name of the product.</summary>
    [JsonPropertyName("productModel")] public string ProductModel { get; set; } = "";
    /// <summary>ID of the parent user who owns this device.</summary>
    [JsonPropertyName("userParentId")] public int UserParentId { get; set; }
    /// <summary>Device operational status (0=Inactive, 1=Online, 2=Offline, 3=Abnormal).</summary>
    [JsonPropertyName("deviceStatus")] public int DeviceStatus { get; set; }
    /// <summary>Timestamp (epoch milliseconds) of the last status change.</summary>
    [JsonPropertyName("statusTime")] public long StatusTime { get; set; }
    /// <summary>Alarm status (0=No alarm, 1=Alarming).</summary>
    [JsonPropertyName("alarmStatus")] public int AlarmStatus { get; set; }
    /// <summary>Timestamp (epoch milliseconds) of the last alarm event.</summary>
    [JsonPropertyName("alarmTime")] public long AlarmTime { get; set; }
    /// <summary>Device battery power level as a percentage.</summary>
    [JsonPropertyName("devicePower")] public int? DevicePower { get; set; }
    /// <summary>Timestamp (epoch milliseconds) when power level was last reported.</summary>
    [JsonPropertyName("powerTime")] public long PowerTime { get; set; }
    /// <summary>Serial number of the gateway this device connects through.</summary>
    [JsonPropertyName("gatewaySn")] public string? GatewaySn { get; set; }
    /// <summary>Product scene configuration code.</summary>
    [JsonPropertyName("productScene")] public int? ProductScene { get; set; }
}
