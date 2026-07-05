using System.Net;
using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class DeviceServiceTests
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly DeviceService _sut;

    public DeviceServiceTests()
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
        _sut = new DeviceService(client);
    }

    [Fact]
    public async Task GetCategoriesAsync_CallsCorrectApiMethod()
    {
        await _sut.GetCategoriesAsync();

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.device.category");
        dict.Should().NotContainKey("bizContent");
    }

    [Fact]
    public async Task GetPageAsync_CallsCorrectApiMethod()
    {
        var request = new GetDevicePageRequest { PageNum = 42 };

        await _sut.GetPageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceInfo.page");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"pageNum\":42");
    }

    [Fact]
    public async Task GetRecordPageAsync_CallsCorrectApiMethod()
    {
        var request = new GetRecordPageRequest { PageNum = 7 };

        await _sut.GetRecordPageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceInfo.recordPage");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"pageNum\":7");
    }

    [Fact]
    public async Task GetSubDevicePageAsync_CallsCorrectApiMethod()
    {
        var request = new GetSubDevicePageRequest { UserDeviceId = 99 };

        await _sut.GetSubDevicePageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceInfo.subPage");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"userDeviceId\":99");
    }
}
