using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Represents an issued device command identified by its record identifier.
/// </summary>
public class IssuedDeviceCmd
{
    /// <summary>
    /// The unique record identifier of the issued command.
    /// </summary>
    [JsonPropertyName("recordId")] public long RecordId { get; set; }
}
