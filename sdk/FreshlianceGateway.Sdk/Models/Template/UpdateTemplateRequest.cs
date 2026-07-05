using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Request DTO for updating an existing device template, including sensor configuration and alarm list.
/// </summary>
public class UpdateTemplateRequest
{
    /// <summary>
    /// The updated sensor configuration for the template.
    /// </summary>
    [JsonPropertyName("sensorConfig")] public UpdateTemplateSensorConfig SensorConfig { get; set; } = new();
    /// <summary>
    /// The updated list of alarm configurations.
    /// </summary>
    [JsonPropertyName("sensorAlarmList")] public List<TemplateAlarmRequest>? SensorAlarmList { get; set; }
    /// <summary>
    /// The configuration identifier of the template to update.
    /// </summary>
    [JsonPropertyName("configId")] public int ConfigId { get; set; }
}
