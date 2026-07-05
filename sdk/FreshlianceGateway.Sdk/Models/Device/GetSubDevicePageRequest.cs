using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Request parameters for paging through the sub-devices of a specific parent device.
/// </summary>
public class GetSubDevicePageRequest
{
    /// <summary>Identifier of the parent user-device association.</summary>
    [JsonPropertyName("userDeviceId")] public int UserDeviceId { get; set; }
    /// <summary>Page number, starting from 1.</summary>
    [JsonPropertyName("pageNum")] public int PageNum { get; set; } = 1;
    /// <summary>Number of items per page.</summary>
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 20;
    /// <summary>Filter by device serial number.</summary>
    [JsonPropertyName("deviceSn")] public string? DeviceSn { get; set; }
    /// <summary>Filter by device name.</summary>
    [JsonPropertyName("deviceName")] public string? DeviceName { get; set; }
    /// <summary>Filter by device status (0=Inactive, 1=Online, 2=Offline, 3=Abnormal).</summary>
    [JsonPropertyName("deviceStatus")] public int? DeviceStatus { get; set; }
    /// <summary>Filter by alarm status (0=No alarm, 1=Alarming).</summary>
    [JsonPropertyName("alarmStatus")] public int? AlarmStatus { get; set; }
    /// <summary>Keyword search across sub-device fields.</summary>
    [JsonPropertyName("keyword")] public string? Keyword { get; set; }
}
