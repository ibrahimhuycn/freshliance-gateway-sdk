using FluentAssertions;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class FreshlianceOptionsTests
{
    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var options = new FreshlianceOptions
        {
            AppId = "test-app",
            PrivateKeyPem = "test-key"
        };

        options.BaseUrl.Should().Be("https://api.freshliance.com/api");
        options.Format.Should().Be("JSON");
        options.Charset.Should().Be("UTF-8");
        options.SignType.Should().Be("RSA2");
        options.Version.Should().Be("1.0");
        options.TimeoutSeconds.Should().Be(30);
    }

    [Fact]
    public void RequiredFields_AppIdAndPrivateKey_Exist()
    {
        var options = new FreshlianceOptions
        {
            AppId = "my-app-id",
            PrivateKeyPem = "my-private-key"
        };

        options.AppId.Should().Be("my-app-id");
        options.PrivateKeyPem.Should().Be("my-private-key");
        options.PublicKeyPem.Should().BeNull();
    }
}
