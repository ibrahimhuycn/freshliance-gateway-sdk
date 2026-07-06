using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Request DTO for deleting an issued device command by its identifier.
/// </summary>
public class DeleteCommandRequest : IBizContent
{
    /// <summary>
    /// The unique identifier of the command to delete.
    /// </summary>
    [JsonPropertyName("id")] public int Id { get; set; }
}
