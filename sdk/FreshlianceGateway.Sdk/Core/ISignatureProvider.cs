namespace FreshlianceGateway.Sdk.Core;

/// <summary>
/// Provides request parameter signing and signature verification for Freshliance API calls.
/// </summary>
public interface ISignatureProvider
{
    /// <summary>
    /// Signs the given parameters and returns the signature string.
    /// </summary>
    /// <param name="parameters">The request parameters to sign.</param>
    /// <returns>The computed signature.</returns>
    string Sign(IDictionary<string, string> parameters);

    /// <summary>
    /// Verifies that the given signature matches the parameters.
    /// </summary>
    /// <param name="parameters">The request parameters.</param>
    /// <param name="signature">The signature to verify.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    bool Verify(IDictionary<string, string> parameters, string signature);
}
