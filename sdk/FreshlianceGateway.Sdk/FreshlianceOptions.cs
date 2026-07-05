using Microsoft.Extensions.Options;

namespace FreshlianceGateway.Sdk;

/// <summary>
/// Configuration options for connecting to the Freshliance API gateway.
/// </summary>
public class FreshlianceOptions
{
    /// <summary>
    /// The application ID assigned by Freshliance.
    /// </summary>
    public required string AppId { get; set; }

    /// <summary>
    /// The RSA private key in PEM format for request signing.
    /// </summary>
    public required string PrivateKeyPem { get; set; }

    /// <summary>
    /// The RSA public key in PEM format for response signature verification. Optional.
    /// </summary>
    public string? PublicKeyPem { get; set; }

    /// <summary>
    /// The base URL of the Freshliance API. Defaults to the production endpoint.
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.freshliance.com/api";

    /// <summary>
    /// The response format. Defaults to JSON.
    /// </summary>
    public string Format { get; set; } = "JSON";

    /// <summary>
    /// The character encoding. Defaults to UTF-8.
    /// </summary>
    public string Charset { get; set; } = "UTF-8";

    /// <summary>
    /// The signature algorithm. Defaults to RSA2.
    /// </summary>
    public string SignType { get; set; } = "RSA2";

    /// <summary>
    /// The API version. Defaults to 1.0.
    /// </summary>
    public string Version { get; set; } = "1.0";

    /// <summary>
    /// The Accept-Language header value for localized error messages. Optional.
    /// </summary>
    public string? AcceptLanguage { get; set; }

    /// <summary>
    /// The HTTP request timeout in seconds. Defaults to 30.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
