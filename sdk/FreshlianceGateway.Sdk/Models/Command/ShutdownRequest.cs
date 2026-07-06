using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Command;

/// <summary>
/// Request DTO for issuing a shutdown command to a device.
/// </summary>
public class ShutdownRequest : IBizContent
{
    /// <summary>
    /// The unique record identifier of the device to shut down.
    /// </summary>
    [JsonPropertyName("recordId")] public int RecordId { get; set; }
}
