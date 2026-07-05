using System.Net;
using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Template;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class ConfigTemplateServiceTests
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly ConfigTemplateService _sut;

    public ConfigTemplateServiceTests()
    {
        _handler = new FakeHttpMessageHandler();

        var signer = Substitute.For<ISignatureProvider>();
        signer.Sign(Arg.Any<IDictionary<string, string>>()).Returns("test-signature");

        var options = Options.Create(new FreshlianceOptions
        {
            AppId = "test-app-id",
            PrivateKeyPem = "dummy-private-key"
        });

        var httpClient = new HttpClient(_handler) { BaseAddress = new Uri("https://api.test.com/api") };
        var client = new FreshlianceClient(httpClient, options, signer);
        _sut = new ConfigTemplateService(client);
    }

    [Fact]
    public async Task GetPageAsync_CallsCorrectApiMethod()
    {
        var request = new GetTemplatePageRequest { PageNum = 3 };

        await _sut.GetPageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.configTemplate.page");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"pageNum\":3");
    }

    [Fact]
    public async Task GetAsync_CallsCorrectApiMethod()
    {
        var request = new GetTemplateDetailRequest { ConfigId = 999L };

        await _sut.GetAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.configTemplate.get");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"configId\":999");
    }

    [Fact]
    public async Task CreateAsync_CallsCorrectApiMethod()
    {
        var request = new CreateTemplateRequest
        {
            SensorConfig = new CreateTemplateSensorConfig { TemplateName = "Test" },
            ProductCode = "PROD-001"
        };

        await _sut.CreateAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.configTemplate.create");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"productCode\":\"PROD-001\"");
    }

    [Fact]
    public async Task UpdateAsync_CallsCorrectApiMethod()
    {
        var request = new UpdateTemplateRequest { ConfigId = 42 };

        await _sut.UpdateAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.configTemplate.update");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"configId\":42");
    }

    [Fact]
    public async Task DeleteAsync_CallsCorrectApiMethod()
    {
        var request = new DeleteTemplateRequest { ConfigId = 77 };

        await _sut.DeleteAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.configTemplate.delete");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"configId\":77");
    }
}
