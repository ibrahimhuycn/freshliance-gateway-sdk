using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response containing detailed information about a sub-device.
/// </summary>
public class SubDeviceInfoResponse
{
    /// <summary>Unique identifier of the user-device association.</summary>
    [JsonPropertyName("userDeviceId")] public int UserDeviceId { get; set; }
    /// <summary>Current record identifier associated with the sub-device.</summary>
    [JsonPropertyName("recordId")] public int RecordId { get; set; }
    /// <summary>User-assigned name for the sub-device.</summary>
    [JsonPropertyName("deviceName")] public string DeviceName { get; set; } = "";
    /// <summary>Serial number of the sub-device.</summary>
    [JsonPropertyName("deviceSn")] public string DeviceSn { get; set; } = "";
    /// <summary>Device operational status (0=Inactive, 1=Online, 2=Offline, 3=Abnormal).</summary>
    [JsonPropertyName("deviceStatus")] public int DeviceStatus { get; set; }
    /// <summary>Alarm status (0=No alarm, 1=Alarming).</summary>
    [JsonPropertyName("alarmStatus")] public int AlarmStatus { get; set; }
    /// <summary>Product code of the device model.</summary>
    [JsonPropertyName("productCode")] public string ProductCode { get; set; } = "";
    /// <summary>Model name of the product.</summary>
    [JsonPropertyName("productModel")] public string ProductModel { get; set; } = "";
    /// <summary>Device battery power level as a percentage.</summary>
    [JsonPropertyName("devicePower")] public int? DevicePower { get; set; }
    /// <summary>Data collection interval in seconds.</summary>
    [JsonPropertyName("collectInterval")] public int CollectInterval { get; set; }
    /// <summary>Product property code indicating device capabilities.</summary>
    [JsonPropertyName("productProperty")] public int ProductProperty { get; set; }
}
