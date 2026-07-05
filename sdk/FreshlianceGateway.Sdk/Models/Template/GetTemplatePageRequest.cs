using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Request DTO for retrieving a paginated list of device templates, optionally filtered by template name.
/// </summary>
public class GetTemplatePageRequest
{
    /// <summary>
    /// The page number to retrieve (1-based).
    /// </summary>
    [JsonPropertyName("pageNum")] public int PageNum { get; set; } = 1;
    /// <summary>
    /// The number of items per page.
    /// </summary>
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 10;
    /// <summary>
    /// Optional filter by template name.
    /// </summary>
    [JsonPropertyName("templateName")] public string? TemplateName { get; set; }
}
