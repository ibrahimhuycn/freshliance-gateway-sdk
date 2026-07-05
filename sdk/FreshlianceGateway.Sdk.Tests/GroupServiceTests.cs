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

public class GroupServiceTests
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly GroupService _sut;

    public GroupServiceTests()
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
        _sut = new GroupService(client);
    }

    [Fact]
    public async Task GetTreeAsync_CallsCorrectApiMethod()
    {
        await _sut.GetTreeAsync();

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceGroup.treeList");
        dict.Should().NotContainKey("bizContent");
    }

    [Fact]
    public async Task GetListAsync_CallsCorrectApiMethod()
    {
        var request = new GetGroupListRequest { ParentId = 5 };

        await _sut.GetListAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceGroup.list");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"parentId\":5");
    }

    [Fact]
    public async Task CreateAsync_CallsCorrectApiMethod()
    {
        var request = new CreateGroupRequest { GroupName = "TestGroup" };

        await _sut.CreateAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceGroup.create");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"groupName\":\"TestGroup\"");
    }

    [Fact]
    public async Task UpdateAsync_CallsCorrectApiMethod()
    {
        var request = new UpdateGroupRequest { GroupId = 10 };

        await _sut.UpdateAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceGroup.update");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"groupId\":10");
    }

    [Fact]
    public async Task DeleteAsync_CallsCorrectApiMethod()
    {
        var request = new DeleteGroupRequest { GroupId = 15 };

        await _sut.DeleteAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_handler.LastRequestContent);

        dict.Should().ContainKey("method").WhoseValue.Should().Be("gw.deviceGroup.delete");
        dict.Should().ContainKey("bizContent").WhoseValue.Should().Contain("\"groupId\":15");
    }
}
