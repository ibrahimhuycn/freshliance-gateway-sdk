using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Request DTO for deleting an issued device command by its identifier.
/// </summary>
public class DeleteCommandRequest
{
    /// <summary>
    /// The unique identifier of the command to delete.
    /// </summary>
    [JsonPropertyName("id")] public int Id { get; set; }
}
