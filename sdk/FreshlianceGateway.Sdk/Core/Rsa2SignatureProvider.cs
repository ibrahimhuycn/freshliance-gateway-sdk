using System.Security.Cryptography;
using System.Text;

namespace FreshlianceGateway.Sdk.Core;

/// <summary>
/// Signs request parameters and verifies signatures using SHA256WithRSA.
/// </summary>
public class Rsa2SignatureProvider : ISignatureProvider
{
    private readonly RSA _privateKey;
    private readonly RSA? _publicKey;

    /// <summary>
    /// Initializes a new instance of <see cref="Rsa2SignatureProvider"/> with a private key and optional public key.
    /// </summary>
    /// <param name="privateKeyPem">The RSA private key in PEM format.</param>
    /// <param name="publicKeyPem">The RSA public key in PEM format. If provided, enables signature verification.</param>
    public Rsa2SignatureProvider(string privateKeyPem, string? publicKeyPem = null)
    {
        _privateKey = RSA.Create();
        _privateKey.ImportFromPem(privateKeyPem);

        if (publicKeyPem is not null)
        {
            _publicKey = RSA.Create();
            _publicKey.ImportFromPem(publicKeyPem);
        }
    }

    /// <summary>
    /// Signs the parameters using the RSA private key with SHA256 and returns the Base64-encoded signature.
    /// </summary>
    /// <param name="parameters">The request parameters to sign.</param>
    /// <returns>The Base64-encoded signature.</returns>
    public string Sign(IDictionary<string, string> parameters)
    {
        var signingString = BuildSigningString(parameters);
        var bytes = Encoding.UTF8.GetBytes(signingString);
        var signature = _privateKey.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// Verifies a signature against the parameters using the RSA public key with SHA256.
    /// </summary>
    /// <param name="parameters">The request parameters to verify.</param>
    /// <param name="signature">The Base64-encoded signature.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no public key has been configured.</exception>
    public bool Verify(IDictionary<string, string> parameters, string signature)
    {
        if (_publicKey is null)
            throw new InvalidOperationException(
                "Public key not configured. Set FreshlianceOptions.PublicKeyPem to enable signature verification.");

        var signingString = BuildSigningString(parameters);
        var bytes = Encoding.UTF8.GetBytes(signingString);
        var sigBytes = Convert.FromBase64String(signature);
        return _publicKey.VerifyData(bytes, sigBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// Builds the signing string by sorting and concatenating parameters, excluding the sign key and empty values.
    /// </summary>
    /// <param name="parameters">The parameters to build the signing string from.</param>
    /// <returns>The signing string in key=value&amp;key=value format.</returns>
    public static string BuildSigningString(IDictionary<string, string> parameters)
    {
        var filtered = parameters
            .Where(kv => kv.Key != "sign" && !string.IsNullOrEmpty(kv.Value))
            .OrderBy(kv => kv.Key, StringComparer.Ordinal)
            .Select(kv => $"{kv.Key}={kv.Value}");

        return string.Join("&", filtered);
    }
}
