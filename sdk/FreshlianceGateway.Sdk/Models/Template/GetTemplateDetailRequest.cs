using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Template;

/// <summary>
/// Request DTO for retrieving the full detail of a device template by its configuration identifier.
/// </summary>
public class GetTemplateDetailRequest : IBizContent
{
    /// <summary>
    /// The configuration identifier of the template to retrieve.
    /// </summary>
    [JsonPropertyName("configId")] public long ConfigId { get; set; }
}
