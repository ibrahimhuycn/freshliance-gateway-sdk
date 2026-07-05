using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Sensor configuration DTO used when creating a new template,
/// specifying start delay, collection interval, category, and template name.
/// </summary>
public class CreateTemplateSensorConfig
{
    /// <summary>
    /// Delay in minutes before data logging starts.
    /// </summary>
    [JsonPropertyName("startDelay")] public int? StartDelay { get; set; }
    /// <summary>
    /// Data collection interval in minutes.
    /// </summary>
    [JsonPropertyName("collectInterval")] public int CollectInterval { get; set; }
    /// <summary>
    /// The category identifier this template belongs to.
    /// </summary>
    [JsonPropertyName("categoryId")] public int? CategoryId { get; set; }
    /// <summary>
    /// The name of the template.
    /// </summary>
    [JsonPropertyName("templateName")] public string TemplateName { get; set; } = "";
}
