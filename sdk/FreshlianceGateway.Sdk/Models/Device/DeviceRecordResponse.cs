using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response containing detailed information about a device record, including configuration and alarm status.
/// </summary>
public class DeviceRecordResponse
{
    /// <summary>Internal record identifier.</summary>
    [JsonPropertyName("recordId")] public int RecordId { get; set; }
    /// <summary>User-device record association identifier.</summary>
    [JsonPropertyName("userDeviceRecordId")] public int UserDeviceRecordId { get; set; }
    /// <summary>Serial number of the device.</summary>
    [JsonPropertyName("deviceSn")] public string DeviceSn { get; set; } = "";
    /// <summary>Internal identifier of the parent device.</summary>
    [JsonPropertyName("parentDeviceId")] public int ParentDeviceId { get; set; }
    /// <summary>Serial number of the parent device.</summary>
    [JsonPropertyName("parentDeviceSn")] public string ParentDeviceSn { get; set; } = "";
    /// <summary>Name of the parent device.</summary>
    [JsonPropertyName("parentDeviceName")] public string ParentDeviceName { get; set; } = "";
    /// <summary>User-assigned name for the device.</summary>
    [JsonPropertyName("deviceName")] public string DeviceName { get; set; } = "";
    /// <summary>Model name of the product.</summary>
    [JsonPropertyName("productModel")] public string ProductModel { get; set; } = "";
    /// <summary>Product type code.</summary>
    [JsonPropertyName("productType")] public int ProductType { get; set; }
    /// <summary>Record status (e.g., 0=Idle, 1=Running, 2=Finished).</summary>
    [JsonPropertyName("recordStatus")] public int RecordStatus { get; set; }
    /// <summary>Time zone identifier used for the record (e.g., "Asia/Shanghai").</summary>
    [JsonPropertyName("timeZone")] public string? TimeZone { get; set; }
    /// <summary>Temperature unit used in the record (e.g., 0=Celsius, 1=Fahrenheit).</summary>
    [JsonPropertyName("temperatureUnit")] public int? TemperatureUnit { get; set; }
    /// <summary>Buzzer status (0=Off, 1=On).</summary>
    [JsonPropertyName("buzzerStatus")] public int? BuzzerStatus { get; set; }
    /// <summary>Data collection interval in seconds.</summary>
    [JsonPropertyName("collectInterval")] public int CollectInterval { get; set; }
    /// <summary>Data upload interval in seconds.</summary>
    [JsonPropertyName("uploadInterval")] public int? UploadInterval { get; set; }
    /// <summary>Delay before the device starts recording in seconds.</summary>
    [JsonPropertyName("startDelay")] public int? StartDelay { get; set; }
    /// <summary>Alarm status (0=No alarm, 1=Alarming).</summary>
    [JsonPropertyName("alarmStatus")] public int AlarmStatus { get; set; }
    /// <summary>Timestamp (epoch milliseconds) of the last alarm event.</summary>
    [JsonPropertyName("alarmTime")] public long? AlarmTime { get; set; }
    /// <summary>Total number of data points collected in this record.</summary>
    [JsonPropertyName("dataCount")] public int DataCount { get; set; }
}
