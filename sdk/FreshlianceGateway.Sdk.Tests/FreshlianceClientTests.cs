using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Device;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class FreshlianceClientTests
{
    private sealed class TestMessageHandler(
        Func<HttpRequestMessage, HttpResponseMessage> handler,
        Action<HttpRequestMessage>? onRequest = null)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            onRequest?.Invoke(request);
            return Task.FromResult(handler(request));
        }
    }

    private static FreshlianceOptions CreateOptions() => new()
    {
        AppId = "test-app-id",
        PrivateKeyPem = "dummy-private-key-pem"
    };

    private static ISignatureProvider CreateSigner()
    {
        var signer = Substitute.For<ISignatureProvider>();
        signer.Sign(Arg.Any<IDictionary<string, string>>()).Returns("test-signature");
        return signer;
    }

    [Fact]
    public async Task PostAsync_WithNoBizContent_BuildsCorrectParameters()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                "{\"code\":\"0\",\"msg\":\"ok\",\"data\":null}",
                Encoding.UTF8, "application/json")
        };
        string? capturedBody = null;
        var handler = new TestMessageHandler(
            _ => response,
            req => capturedBody = req.Content!.ReadAsStringAsync().GetAwaiter().GetResult());
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.test.com/api") };
        var signer = CreateSigner();
        var options = Options.Create(CreateOptions());

        var client = new FreshlianceClient(httpClient, options, signer);
        var ct = TestContext.Current.CancellationToken;
        await client.PostAsync<object>("test.method", null, ct);

        capturedBody.Should().NotBeNull();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(capturedBody);

        dict.Should().NotBeNull();
        dict!.Should().ContainKey("appId").WhoseValue.Should().Be("test-app-id");
        dict.Should().ContainKey("method").WhoseValue.Should().Be("test.method");
        dict.Should().ContainKey("format").WhoseValue.Should().Be("JSON");
        dict.Should().ContainKey("charset").WhoseValue.Should().Be("UTF-8");
        dict.Should().ContainKey("signType").WhoseValue.Should().Be("RSA2");
        dict.Should().ContainKey("version").WhoseValue.Should().Be("1.0");
        dict.Should().ContainKey("timestamp");
        dict.Should().ContainKey("sign").WhoseValue.Should().Be("test-signature");
        dict.Should().NotContainKey("bizContent");
    }

    [Fact]
    public async Task PostAsync_WithBizContent_IncludesBizContentInRequest()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                "{\"code\":\"0\",\"msg\":\"ok\",\"data\":null}",
                Encoding.UTF8, "application/json")
        };
        string? capturedBody = null;
        var handler = new TestMessageHandler(
            _ => response,
            req => capturedBody = req.Content!.ReadAsStringAsync().GetAwaiter().GetResult());
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.test.com/api") };
        var signer = CreateSigner();
        var options = Options.Create(CreateOptions());

        var client = new FreshlianceClient(httpClient, options, signer);
        var bizContent = new GetDevicePageRequest { PageNum = 1, PageSize = 10 };
        var ct = TestContext.Current.CancellationToken;
        await client.PostAsync<object>("test.method", bizContent, ct);

        capturedBody.Should().NotBeNull();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(capturedBody);

        dict.Should().ContainKey("bizContent")
            .WhoseValue.Should().Be("{\"pageNum\":1,\"pageSize\":10}");
    }

    [Fact]
    public async Task PostAsync_SuccessResponse_ParsesCorrectly()
    {
        var responseJson =
            "{\"code\":\"0\",\"msg\":\"success\",\"data\":{\"name\":\"test-item\",\"value\":42}}";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };
        var handler = new TestMessageHandler(_ => response);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.test.com/api") };
        var signer = CreateSigner();
        var options = Options.Create(CreateOptions());

        var client = new FreshlianceClient(httpClient, options, signer);
        var ct = TestContext.Current.CancellationToken;
        var result = await client.PostAsync<TestData>("test.method", null, ct);

        result.Should().NotBeNull();
        result.Code.Should().Be("0");
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("test-item");
        result.Data.Value.Should().Be(42);
    }

    [Fact]
    public async Task PostAsync_ErrorResponse_ThrowsFreshlianceException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(
                "{\"code\":\"40001\",\"msg\":\"invalid parameter\"}",
                Encoding.UTF8, "application/json")
        };
        var handler = new TestMessageHandler(_ => response);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.test.com/api") };
        var signer = CreateSigner();
        var options = Options.Create(CreateOptions());
        var ct = TestContext.Current.CancellationToken;

        var client = new FreshlianceClient(httpClient, options, signer);

        var act = async () => await client.PostAsync<object>("test.method", null, ct);

        await act.Should().ThrowAsync<FreshlianceException>()
            .Where(ex => ex.Code == "400");
    }

    [Fact]
    public async Task PostAsync_EmptyResponse_ThrowsFreshlianceException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };
        var handler = new TestMessageHandler(_ => response);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.test.com/api") };
        var signer = CreateSigner();
        var options = Options.Create(CreateOptions());
        var ct = TestContext.Current.CancellationToken;

        var client = new FreshlianceClient(httpClient, options, signer);

        var act = async () => await client.PostAsync<object>("test.method", null, ct);

        await act.Should().ThrowAsync<FreshlianceException>()
            .Where(ex => ex.Code == "99999" && ex.Message.Contains("Empty response"));
    }

    private class TestData
    {
        public string Name { get; set; } = "";
        public int Value { get; set; }
    }
}
