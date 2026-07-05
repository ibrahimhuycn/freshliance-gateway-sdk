using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response item for a single device record in a paged record list, including the record details, sensor configuration, and sub-device data.
/// </summary>
public class RecordPageItemResponse
{
    /// <summary>The device record details.</summary>
    [JsonPropertyName("deviceRecord")] public DeviceRecordResponse DeviceRecord { get; set; } = new();
    /// <summary>Sensor threshold configurations for the device.</summary>
    [JsonPropertyName("sensorConfig")] public List<SensorConfigResponse>? SensorConfig { get; set; }
    /// <summary>Latest data readings from sub-devices attached to this device.</summary>
    [JsonPropertyName("subDeviceLastDataList")] public List<SubDeviceDataResponse> SubDeviceLastDataList { get; set; } = [];
}
