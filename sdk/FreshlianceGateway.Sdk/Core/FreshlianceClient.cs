using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace FreshlianceGateway.Sdk.Core;

/// <summary>
/// Client for making signed HTTP POST requests to the Freshliance API gateway.
/// </summary>
public class FreshlianceClient
{
    private readonly HttpClient _http;
    private readonly FreshlianceOptions _options;
    private readonly ISignatureProvider _signer;

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new BizContentJsonConverter() }
    };

    /// <summary>
    /// Initializes a new instance of <see cref="FreshlianceClient"/>.
    /// </summary>
    /// <param name="http">The <see cref="HttpClient"/> used for requests.</param>
    /// <param name="options">The Freshliance API configuration options.</param>
    /// <param name="signer">The signature provider for request signing.</param>
    public FreshlianceClient(HttpClient http, IOptions<FreshlianceOptions> options, ISignatureProvider signer)
    {
        _http = http;
        _options = options.Value;
        _signer = signer;
    }

    /// <summary>
    /// Sends a signed POST request with the given API method and optional business content.
    /// </summary>
    /// <typeparam name="T">The expected response data type.</typeparam>
    /// <param name="method">The API method name.</param>
    /// <param name="bizContent">The business content payload, or null.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The deserialized API response.</returns>
    /// <exception cref="FreshlianceException">Thrown when the HTTP response is not successful or the body is empty.</exception>
    public async Task<FreshlianceResponse<T>> PostAsync<T>(string method, object? bizContent,
        CancellationToken ct = default)
    {
        var parameters = BuildParameters(method, bizContent);
        var sign = _signer.Sign(parameters);
        parameters["sign"] = sign;

        var body = JsonSerializer.Serialize(parameters, JsonOptions);
        using var content = new StringContent(body, Encoding.UTF8, "application/json");

        using var response = await _http.PostAsync("", content, ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            throw new FreshlianceException(
                $"{(int)response.StatusCode}",
                null,
                $"HTTP {response.StatusCode}",
                errorBody
            );
        }

        var result = await response.Content
            .ReadFromJsonAsync<FreshlianceResponse<T>>(JsonOptions, ct)
            .ConfigureAwait(false);

        return result ?? throw new FreshlianceException("99999", null, "Empty response", null);
    }

    private Dictionary<string, string> BuildParameters(string method, object? bizContent)
    {
        var parameters = new Dictionary<string, string>
        {
            ["appId"] = _options.AppId,
            ["method"] = method,
            ["format"] = _options.Format,
            ["charset"] = _options.Charset,
            ["signType"] = _options.SignType,
            ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            ["version"] = _options.Version
        };

        if (bizContent is not null)
        {
            parameters["bizContent"] = JsonSerializer.Serialize(bizContent, JsonOptions);
        }

        return parameters;
    }
}
