using System.Net;
using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Command;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class RemoteCommandServiceTests
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly RemoteCommandService _sut;

    public RemoteCommandServiceTests()
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
        _sut = new RemoteCommandService(client);
    }

    [Fact]
    public async Task UpdateParameterAsync_CallsCorrectApiMethod()
    {
        var request = new UpdateParameterRequest { RecordId = 1 };

        await _sut.UpdateParameterAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceCmd.updateParameter");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"recordId\":1");
    }

    [Fact]
    public async Task SaveDataShutdownAsync_CallsCorrectApiMethod()
    {
        var request = new ShutdownRequest { RecordId = 2 };

        await _sut.SaveDataShutdownAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceCmd.saveDataShutdown");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"recordId\":2");
    }

    [Fact]
    public async Task DirectShutdownAsync_CallsCorrectApiMethod()
    {
        var request = new ShutdownRequest { RecordId = 3 };

        await _sut.DirectShutdownAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceCmd.directShutdown");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"recordId\":3");
    }

    [Fact]
    public async Task SaveDataConfigAsync_CallsCorrectApiMethod()
    {
        var request = new SaveDataConfigRequest
        {
            IssuedDeviceCmd = new IssuedDeviceCmd { RecordId = 1 },
            CollectInterval = 5
        };

        await _sut.SaveDataConfigAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceCmd.saveDataConfig");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"collectInterval\":5");
    }

    [Fact]
    public async Task DirectConfigAsync_CallsCorrectApiMethod()
    {
        var request = new DirectConfigRequest
        {
            IssuedDeviceCmd = new IssuedDeviceCmd { RecordId = 1 },
            CollectInterval = 10
        };

        await _sut.DirectConfigAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceCmd.directConfig");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"collectInterval\":10");
    }

    [Fact]
    public async Task DeleteCommandAsync_CallsCorrectApiMethod()
    {
        var request = new DeleteCommandRequest { Id = 100 };

        await _sut.DeleteCommandAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceCmd.delete");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"id\":100");
    }
}
