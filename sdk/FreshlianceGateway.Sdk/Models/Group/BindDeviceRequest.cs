using System.Text.Json.Serialization;
using FreshlianceGateway.Sdk.Core;

namespace FreshlianceGateway.Sdk.Models.Group;

/// <summary>
/// Request model for binding one or more devices to a group.
/// </summary>
public class BindDeviceRequest : IBizContent
{
    /// <summary>
    /// The array of user-device association identifiers to bind.
    /// </summary>
    [JsonPropertyName("userDeviceIds")] public int[] UserDeviceIds { get; set; } = [];
    /// <summary>
    /// The target group identifier to bind the devices to.
    /// </summary>
    [JsonPropertyName("groupId")] public int GroupId { get; set; }
}
