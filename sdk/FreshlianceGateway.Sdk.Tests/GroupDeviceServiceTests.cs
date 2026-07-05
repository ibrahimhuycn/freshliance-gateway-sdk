using System.Net;
using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Group;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class GroupDeviceServiceTests
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly GroupDeviceService _sut;

    public GroupDeviceServiceTests()
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
        _sut = new GroupDeviceService(client);
    }

    [Fact]
    public async Task GetUnallocatedPageAsync_CallsCorrectApiMethod()
    {
        var request = new GetUnallocatedDeviceRequest { GroupId = 20 };

        await _sut.GetUnallocatedPageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.groupDevice.pageUnAllocatedDevice");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"groupId\":20");
    }

    [Fact]
    public async Task GetAllocatedPageAsync_CallsCorrectApiMethod()
    {
        var request = new GetAllocatedDeviceRequest { GroupId = 25 };

        await _sut.GetAllocatedPageAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.groupDevice.pageAllocatedDevice");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"groupId\":25");
    }

    [Fact]
    public async Task UnbindAsync_CallsCorrectApiMethod()
    {
        var request = new UnbindDeviceRequest { UserDeviceId = 30 };

        await _sut.UnbindAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.groupDevice.unbindDevice");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"userDeviceId\":30");
    }

    [Fact]
    public async Task BindAsync_CallsCorrectApiMethod()
    {
        var request = new BindDeviceRequest { GroupId = 35, UserDeviceIds = [1, 2] };

        await _sut.BindAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.groupDevice.bindDevice");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"groupId\":35");
    }
}
