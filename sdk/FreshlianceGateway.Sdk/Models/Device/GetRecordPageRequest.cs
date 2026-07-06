using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Request parameters for paging through device records with optional filters.
/// </summary>
public class GetRecordPageRequest : IBizContent
{
    /// <summary>Page number, starting from 1.</summary>
    [JsonPropertyName("pageNum")] public int PageNum { get; set; } = 1;
    /// <summary>Number of items per page.</summary>
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 20;
    /// <summary>Filter by internal device identifier.</summary>
    [JsonPropertyName("deviceId")] public int? DeviceId { get; set; }
    /// <summary>Filter by device serial number.</summary>
    [JsonPropertyName("deviceSn")] public string? DeviceSn { get; set; }
    /// <summary>Filter by record status.</summary>
    [JsonPropertyName("recordStatus")] public int? RecordStatus { get; set; }
    /// <summary>Filter by device name.</summary>
    [JsonPropertyName("deviceName")] public string? DeviceName { get; set; }
    /// <summary>Filter records with data later than or equal to this timestamp (epoch milliseconds).</summary>
    [JsonPropertyName("startTime")] public long? StartTime { get; set; }
    /// <summary>Filter records with data earlier than or equal to this timestamp (epoch milliseconds).</summary>
    [JsonPropertyName("endTime")] public long? EndTime { get; set; }
    /// <summary>Filter by product type code.</summary>
    [JsonPropertyName("productType")] public int? ProductType { get; set; }
    /// <summary>Keyword search across record fields.</summary>
    [JsonPropertyName("keyword")] public string? Keyword { get; set; }
}
