using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for retrieving devices already allocated to a group.
/// </summary>
public class GetAllocatedDeviceRequest : IBizContent
{
    /// <summary>
    /// The page number for pagination. Default is 1.
    /// </summary>
    [JsonPropertyName("pageNum")] public int PageNum { get; set; } = 1;
    /// <summary>
    /// The number of items per page. Default is 10.
    /// </summary>
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 10;
    /// <summary>
    /// The group identifier to query allocated devices from.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
    /// <summary>
    /// Optional filter by device serial number.
    /// </summary>
    [JsonPropertyName("deviceSn")] public string? DeviceSn { get; set; }
    /// <summary>
    /// Optional filter by device name.
    /// </summary>
    [JsonPropertyName("deviceName")] public string? DeviceName { get; set; }
    /// <summary>
    /// Optional filter by device status.
    /// </summary>
    [JsonPropertyName("deviceStatus")] public int? DeviceStatus { get; set; }
    /// <summary>
    /// Optional keyword search filter.
    /// </summary>
    [JsonPropertyName("keyword")] public string? Keyword { get; set; }
}
