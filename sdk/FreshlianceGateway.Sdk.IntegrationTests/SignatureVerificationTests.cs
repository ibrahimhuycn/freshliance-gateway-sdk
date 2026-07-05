using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Services;
using Xunit;

namespace FreshlianceGateway.Sdk.IntegrationTests;

public class SignatureVerificationTests : IClassFixture<WireMockServerFixture>
{
    private readonly WireMockServerFixture _fixture;

    public SignatureVerificationTests(WireMockServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task RequestIncludesValidSignature()
    {
        _fixture.ResetAndStubSuccess("""
            {
                "code": "0",
                "msg": "success",
                "sign": "mock-response-sign",
                "data": { "email": "sig@test.com", "timeZone": "UTC", "language": 1, "dateFormat": 1, "temperatureUnit": 1, "chnSmsNum": 0, "intSmsNum": 0, "chnVoiceNum": 0, "intVoiceNum": 0 }
            }
            """);

        var client = _fixture.CreateClient();
        var service = new UserService(client);

        await service.GetAsync(TestContext.Current.CancellationToken);

        var body = _fixture.LastRequestBody();
        body.Should().NotBeNull();

        using var doc = JsonDocument.Parse(body!);
        var root = doc.RootElement;

        root.TryGetProperty("sign", out var signProp).Should().BeTrue();
        signProp.GetString().Should().NotBeNullOrEmpty();

        root.TryGetProperty("method", out var methodProp).Should().BeTrue();
        methodProp.GetString().Should().Be("gw.userInfo.get");

        root.TryGetProperty("appId", out var appIdProp).Should().BeTrue();
        appIdProp.GetString().Should().Be(WireMockServerFixture.AppId);

        var parameters = new Dictionary<string, string>();
        foreach (var prop in root.EnumerateObject())
        {
            var val = prop.Value.ValueKind == JsonValueKind.String
                ? prop.Value.GetString()!
                : prop.Value.GetRawText();
            parameters[prop.Name] = val;
        }

        var signer = new Rsa2SignatureProvider(_fixture.PrivateKeyPem, _fixture.PublicKeyPem);
        var signValue = parameters["sign"];
        parameters.Remove("sign");
        signer.Verify(parameters, signValue).Should().BeTrue();
    }

    [Fact]
    public void ResponseSignature_CanBeVerified()
    {
        var responseParams = new Dictionary<string, string>
        {
            ["code"] = "0",
            ["msg"] = "success",
            ["data"] = """{"email":"verify@freshliance.com","timeZone":"UTC"}"""
        };

        var signer = new Rsa2SignatureProvider(_fixture.PrivateKeyPem, _fixture.PublicKeyPem);
        var signature = signer.Sign(responseParams);
        signature.Should().NotBeNullOrEmpty();

        var isValid = signer.Verify(responseParams, signature);
        isValid.Should().BeTrue();

        var tampered = new Dictionary<string, string>(responseParams)
        {
            ["code"] = "40000"
        };
        var tamperedValid = signer.Verify(tampered, signature);
        tamperedValid.Should().BeFalse();
    }
}
