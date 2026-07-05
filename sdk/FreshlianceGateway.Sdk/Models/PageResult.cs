using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk;

/// <summary>
/// Represents a paginated result set from the Freshliance API.
/// </summary>
/// <typeparam name="T">The type of items in each row.</typeparam>
public class PageResult<T>
{
    /// <summary>
    /// The total number of records available.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    /// The rows in the current page.
    /// </summary>
    [JsonPropertyName("rows")]
    public List<T> Rows { get; set; } = [];
}
