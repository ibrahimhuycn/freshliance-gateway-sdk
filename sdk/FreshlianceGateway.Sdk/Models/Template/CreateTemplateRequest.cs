using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Request DTO for creating a new device template with sensor configuration,
/// alarm list, category, and product code.
/// </summary>
public class CreateTemplateRequest : IBizContent
{
    /// <summary>
    /// The sensor configuration for the new template.
    /// </summary>
    [JsonPropertyName("sensorConfig")] public CreateTemplateSensorConfig SensorConfig { get; set; } = new();
    /// <summary>
    /// List of alarm configurations to associate with the template.
    /// </summary>
    [JsonPropertyName("sensorAlarmList")] public List<TemplateAlarmRequest>? SensorAlarmList { get; set; }
    /// <summary>
    /// The category identifier this template belongs to.
    /// </summary>
    [JsonPropertyName("categoryId")] public int CategoryId { get; set; }
    /// <summary>
    /// The product code associated with this template.
    /// </summary>
    [JsonPropertyName("productCode")] public string ProductCode { get; set; } = "";
}
