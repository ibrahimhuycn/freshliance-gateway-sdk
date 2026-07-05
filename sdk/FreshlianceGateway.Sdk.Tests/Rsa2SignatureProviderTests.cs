using System.Security.Cryptography;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class Rsa2SignatureProviderTests
{
    [Fact]
    public void BuildSigningString_FiltersSignField()
    {
        var parameters = new Dictionary<string, string>
        {
            ["appId"] = "123",
            ["sign"] = "abc123",
            ["method"] = "test.method"
        };

        var result = Rsa2SignatureProvider.BuildSigningString(parameters);

        result.Should().NotContain("sign");
        result.Should().Be("appId=123&method=test.method");
    }

    [Fact]
    public void BuildSigningString_FiltersEmptyValues()
    {
        var parameters = new Dictionary<string, string>
        {
            ["appId"] = "123",
            ["empty"] = "",
            ["nullLike"] = null!,
            ["method"] = "test.method"
        };

        var result = Rsa2SignatureProvider.BuildSigningString(parameters);

        result.Should().NotContain("empty");
        result.Should().NotContain("nullLike");
        result.Should().Be("appId=123&method=test.method");
    }

    [Fact]
    public void BuildSigningString_SortsByAscii()
    {
        var parameters = new Dictionary<string, string>
        {
            ["z"] = "zval",
            ["A"] = "Aval",
            ["1"] = "1val",
            ["a"] = "aval"
        };

        var result = Rsa2SignatureProvider.BuildSigningString(parameters);

        result.Should().Be("1=1val&A=Aval&a=aval&z=zval");
    }

    [Fact]
    public void BuildSigningString_MatchesDocsExample()
    {
        var parameters = new Dictionary<string, string>
        {
            ["appId"] = "658409073956360262328652394",
            ["bizContent"] = "{\"pageNum\":1,\"pageSize\":10}",
            ["charset"] = "UTF-8",
            ["format"] = "JSON",
            ["method"] = "tracker.userDevice.page",
            ["signType"] = "RSA2",
            ["timestamp"] = "1747208216323",
            ["version"] = "1.0"
        };

        var result = Rsa2SignatureProvider.BuildSigningString(parameters);

        result.Should().Be(
            "appId=658409073956360262328652394&" +
            "bizContent={\"pageNum\":1,\"pageSize\":10}&" +
            "charset=UTF-8&" +
            "format=JSON&" +
            "method=tracker.userDevice.page&" +
            "signType=RSA2&" +
            "timestamp=1747208216323&" +
            "version=1.0");
    }

    [Fact]
    public void Sign_ProducesBase64String()
    {
        var (privateKeyPem, _) = GenerateKeyPair();
        var provider = new Rsa2SignatureProvider(privateKeyPem);

        var parameters = new Dictionary<string, string>
        {
            ["appId"] = "123",
            ["method"] = "test.method",
            ["timestamp"] = "1234567890",
            ["version"] = "1.0"
        };

        var signature = provider.Sign(parameters);

        signature.Should().NotBeNullOrEmpty();
        var act = () => Convert.FromBase64String(signature);
        act.Should().NotThrow();
    }

    [Fact]
    public void SignAndVerify_RoundTrip()
    {
        var (privateKeyPem, publicKeyPem) = GenerateKeyPair();
        var provider = new Rsa2SignatureProvider(privateKeyPem, publicKeyPem);

        var parameters = new Dictionary<string, string>
        {
            ["appId"] = "123",
            ["method"] = "test.method",
            ["timestamp"] = "1234567890",
            ["version"] = "1.0"
        };

        var signature = provider.Sign(parameters);

        var isValid = provider.Verify(parameters, signature);
        isValid.Should().BeTrue();
    }

    [Fact]
    public void Verify_ThrowsWhenNoPublicKeyConfigured()
    {
        var (privateKeyPem, _) = GenerateKeyPair();
        var provider = new Rsa2SignatureProvider(privateKeyPem);

        var parameters = new Dictionary<string, string>
        {
            ["appId"] = "123",
            ["method"] = "test.method"
        };

        var act = () => provider.Verify(parameters, "dummySignature");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Public key not configured*");
    }

    private static (string PrivateKeyPem, string PublicKeyPem) GenerateKeyPair()
    {
        using var rsa = RSA.Create(2048);
        var privateKey = rsa.ExportRSAPrivateKeyPem();
        var publicKey = rsa.ExportRSAPublicKeyPem();
        return (privateKey, publicKey);
    }
}
