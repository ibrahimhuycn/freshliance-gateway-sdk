namespace FreshlianceGateway.Sdk;

/// <summary>
/// Represents an error returned by the Freshliance API.
/// </summary>
public class FreshlianceException : Exception
{
    /// <summary>
    /// The top-level error code from the API response.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// The sub-error code from the API response, if any.
    /// </summary>
    public string? SubCode { get; }

    /// <summary>
    /// The sub-error message from the API response, if any.
    /// </summary>
    public string? SubMsg { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="FreshlianceException"/> with error details.
    /// </summary>
    /// <param name="code">The top-level error code.</param>
    /// <param name="subCode">The sub-error code, or null.</param>
    /// <param name="msg">The error message.</param>
    /// <param name="subMsg">The sub-error message, or null.</param>
    public FreshlianceException(string code, string? subCode, string? msg, string? subMsg)
        : base($"[{code}] {msg}" + (subCode is not null ? $" — {subCode}: {subMsg}" : ""))
    {
        Code = code;
        SubCode = subCode;
        SubMsg = subMsg;
    }
}
