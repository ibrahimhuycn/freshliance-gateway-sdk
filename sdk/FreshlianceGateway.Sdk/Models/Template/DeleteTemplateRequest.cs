using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Request DTO for deleting a device template by its configuration identifier.
/// </summary>
public class DeleteTemplateRequest
{
    /// <summary>
    /// The configuration identifier of the template to delete.
    /// </summary>
    [JsonPropertyName("configId")] public int ConfigId { get; set; }
}
