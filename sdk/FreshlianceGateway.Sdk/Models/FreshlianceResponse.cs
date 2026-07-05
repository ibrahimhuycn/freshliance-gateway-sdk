using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk;

/// <summary>
/// Represents a response from the Freshliance API.
/// </summary>
/// <typeparam name="T">The type of the response data payload.</typeparam>
public class FreshlianceResponse<T>
{
    /// <summary>
    /// The top-level response code. A value of "0" indicates success.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";

    /// <summary>
    /// The response message.
    /// </summary>
    [JsonPropertyName("msg")]
    public string Msg { get; set; } = "";

    /// <summary>
    /// The sub-error code, if present.
    /// </summary>
    [JsonPropertyName("subCode")]
    public string? SubCode { get; set; }

    /// <summary>
    /// The sub-error message, if present.
    /// </summary>
    [JsonPropertyName("subMsg")]
    public string? SubMsg { get; set; }

    /// <summary>
    /// The response signature.
    /// </summary>
    [JsonPropertyName("sign")]
    public string Sign { get; set; } = "";

    /// <summary>
    /// The response data payload.
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    /// Gets a value indicating whether the response indicates success (code "0").
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Code == "0";

    /// <summary>
    /// Throws a <see cref="FreshlianceException"/> if the response is not successful.
    /// </summary>
    /// <exception cref="FreshlianceException">Thrown when <see cref="IsSuccess"/> is false.</exception>
    public void EnsureSuccess()
    {
        if (!IsSuccess)
            throw new FreshlianceException(Code, SubCode, Msg, SubMsg);
    }
}
