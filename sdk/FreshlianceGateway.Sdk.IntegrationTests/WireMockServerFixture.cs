using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;
using FreshlianceGateway.Sdk.Core;
using Microsoft.Extensions.Options;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace FreshlianceGateway.Sdk.IntegrationTests;

public class WireMockServerFixture : IAsyncLifetime, IDisposable
{
    public WireMockServer Server { get; private set; } = null!;
    public string PrivateKeyPem { get; private set; } = "";
    public string PublicKeyPem { get; private set; } = "";
    public const string AppId = "test-app-id";

    public string BaseAddress { get; private set; } = "";

    public static (string privateKey, string publicKey) GenerateRsaKeyPair()
    {
        using var rsa = RSA.Create(2048);
        return (rsa.ExportRSAPrivateKeyPem(), rsa.ExportRSAPublicKeyPem());
    }

    public async ValueTask InitializeAsync()
    {
        var port = GetFreeTcpPort();
        Server = WireMockServer.Start(port);
        BaseAddress = $"http://localhost:{port}/api";

        (PrivateKeyPem, PublicKeyPem) = GenerateRsaKeyPair();
        await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        Dispose();
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Server?.Dispose();
    }

    public void ResetAndStubSuccess(string body)
    {
        Server.Reset();
        Server
            .Given(Request.Create().WithPath("/api").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(body));
    }

    public void ResetAndStubError(int statusCode, string body)
    {
        Server.Reset();
        Server
            .Given(Request.Create().WithPath("/api").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(statusCode)
                .WithHeader("Content-Type", "application/json")
                .WithBody(body));
    }

    public FreshlianceClient CreateClient()
    {
        var options = Options.Create(new FreshlianceOptions
        {
            AppId = AppId,
            PrivateKeyPem = PrivateKeyPem,
            PublicKeyPem = PublicKeyPem,
            BaseUrl = BaseAddress
        });

        var signer = new Rsa2SignatureProvider(PrivateKeyPem, PublicKeyPem);
        var httpClient = new HttpClient { BaseAddress = new Uri(BaseAddress) };
        return new FreshlianceClient(httpClient, options, signer);
    }

    public string? LastRequestBody()
    {
        var entry = Server.LogEntries.LastOrDefault();
        return entry?.RequestMessage?.Body;
    }

    public JsonDocument? LastRequestBodyAsJson()
    {
        var body = LastRequestBody();
        if (body is null)
            return null;
        return JsonDocument.Parse(body);
    }

    private static int GetFreeTcpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
