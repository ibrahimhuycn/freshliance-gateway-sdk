using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Sensor configuration DTO used when updating an existing template,
/// specifying config ID, start delay, collection interval, and template name.
/// </summary>
public class UpdateTemplateSensorConfig
{
    /// <summary>
    /// The configuration identifier of the template to update.
    /// </summary>
    [JsonPropertyName("configId")] public int ConfigId { get; set; }
    /// <summary>
    /// Delay in minutes before data logging starts.
    /// </summary>
    [JsonPropertyName("startDelay")] public int? StartDelay { get; set; }
    /// <summary>
    /// Data collection interval in minutes.
    /// </summary>
    [JsonPropertyName("collectInterval")] public int CollectInterval { get; set; }
    /// <summary>
    /// The name of the template.
    /// </summary>
    [JsonPropertyName("templateName")] public string TemplateName { get; set; } = "";
}
