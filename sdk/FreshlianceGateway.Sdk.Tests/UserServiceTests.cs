using System.Net;
using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.User;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class UserServiceTests
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly UserService _sut;

    public UserServiceTests()
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
        _sut = new UserService(client);
    }

    [Fact]
    public async Task GetAsync_CallsCorrectApiMethod()
    {
        await _sut.GetAsync();

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.userInfo.get");
        dict.Should().NotContainKey("bizContent");
    }

    [Fact]
    public async Task UpdateAsync_CallsCorrectApiMethod()
    {
        var request = new UpdateUserInfoRequest { Nickname = "TestUser" };

        await _sut.UpdateAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.userInfo.update");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"nickname\":\"TestUser\"");
    }
}
