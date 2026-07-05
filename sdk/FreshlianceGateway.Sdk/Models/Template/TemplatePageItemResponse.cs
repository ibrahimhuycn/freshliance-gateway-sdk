using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Response DTO representing a single template item in a paginated template list.
/// </summary>
public class TemplatePageItemResponse
{
    /// <summary>
    /// The unique configuration identifier for the template.
    /// </summary>
    [JsonPropertyName("configId")] public int ConfigId { get; set; }
    /// <summary>
    /// Delay in minutes before data logging starts.
    /// </summary>
    [JsonPropertyName("startDelay")] public int StartDelay { get; set; }
    /// <summary>
    /// Data collection interval in minutes.
    /// </summary>
    [JsonPropertyName("collectInterval")] public int CollectInterval { get; set; }
    /// <summary>
    /// The name of the template.
    /// </summary>
    [JsonPropertyName("templateName")] public string TemplateName { get; set; } = "";
    /// <summary>
    /// The name of the category this template belongs to.
    /// </summary>
    [JsonPropertyName("categoryName")] public string CategoryName { get; set; } = "";
    /// <summary>
    /// The unique identifier of the category.
    /// </summary>
    [JsonPropertyName("categoryId")] public int CategoryId { get; set; }
    /// <summary>
    /// The creation timestamp of the template.
    /// </summary>
    [JsonPropertyName("createTime")] public string? CreateTime { get; set; }
    /// <summary>
    /// List of probe configurations associated with this template.
    /// </summary>
    [JsonPropertyName("sensorConfigProbeList")] public List<TemplateProbeResponse>? SensorConfigProbeList { get; set; }
}
