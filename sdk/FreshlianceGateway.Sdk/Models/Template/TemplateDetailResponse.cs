using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Response DTO containing the full detail of a template, including its sensor configuration,
/// alarm list, and category information.
/// </summary>
public class TemplateDetailResponse
{
    /// <summary>
    /// The sensor configuration for this template.
    /// </summary>
    [JsonPropertyName("sensorConfig")] public TemplateSensorConfigResponse SensorConfig { get; set; } = new();
    /// <summary>
    /// List of alarm configurations associated with this template.
    /// </summary>
    [JsonPropertyName("sensorAlarmList")] public List<TemplateAlarmResponse>? SensorAlarmList { get; set; }
    /// <summary>
    /// The name of the category this template belongs to.
    /// </summary>
    [JsonPropertyName("categoryName")] public string CategoryName { get; set; } = "";
    /// <summary>
    /// The unique identifier of the category.
    /// </summary>
    [JsonPropertyName("categoryId")] public int CategoryId { get; set; }
}
