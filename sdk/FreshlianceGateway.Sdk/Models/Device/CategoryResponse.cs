using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Models.Device;

/// <summary>
/// Response representing a product category with its temperature thresholds and sensor configuration.
/// </summary>
public class CategoryResponse
{
    /// <summary>Unique identifier for the category.</summary>
    [JsonPropertyName("categoryId")] public int CategoryId { get; set; }
    /// <summary>Display name of the category.</summary>
    [JsonPropertyName("categoryName")] public string CategoryName { get; set; } = "";
    /// <summary>Internal temperature high alarm threshold.</summary>
    [JsonPropertyName("inTemHigh")] public double InTemHigh { get; set; }
    /// <summary>Internal temperature low alarm threshold.</summary>
    [JsonPropertyName("inTemLow")] public double InTemLow { get; set; }
    /// <summary>External temperature high alarm threshold.</summary>
    [JsonPropertyName("extTemHigh")] public double ExtTemHigh { get; set; }
    /// <summary>External temperature low alarm threshold.</summary>
    [JsonPropertyName("extTemLow")] public double ExtTemLow { get; set; }
    /// <summary>Number of product sensors configured for this category.</summary>
    [JsonPropertyName("productSensor")] public int ProductSensor { get; set; }
    /// <summary>Optional remark or notes for the category.</summary>
    [JsonPropertyName("remark")] public string? Remark { get; set; }
}
