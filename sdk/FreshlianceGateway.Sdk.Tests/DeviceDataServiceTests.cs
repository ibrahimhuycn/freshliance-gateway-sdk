using System.Net;
using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Data;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class DeviceDataServiceTests
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly DeviceDataService _sut;

    public DeviceDataServiceTests()
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
        _sut = new DeviceDataService(client);
    }

    [Fact]
    public async Task GetDataPageAsync_CallsCorrectApiMethod()
    {
        var request = new GetDeviceDataRequest { RecordId = 123 };

        await _sut.GetDataPageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceData.page");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"recordId\":123");
    }

    [Fact]
    public async Task GetAlarmPageAsync_CallsCorrectApiMethod()
    {
        var request = new GetAlarmDataRequest { RecordId = 456 };

        await _sut.GetAlarmPageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceAlarmData.page");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"recordId\":456");
    }
}
