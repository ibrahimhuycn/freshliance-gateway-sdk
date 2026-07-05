using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for unbinding a device from a group.
/// </summary>
public class UnbindDeviceRequest
{
    /// <summary>
    /// The user-device association identifier to unbind.
    /// </summary>
    [JsonPropertyName("userDeviceId")] public int UserDeviceId { get; set; }
    /// <summary>
    /// The group identifier to unbind the device from.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
}
