using System.Net;

namespace FreshlianceGateway.Sdk.Tests;

internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private string _lastRequestContent = string.Empty;

    public string LastRequestContent => _lastRequestContent;

    public HttpResponseMessage Response { get; set; } = new(HttpStatusCode.OK)
    {
        Content = new StringContent("{\"code\":\"0\",\"msg\":\"ok\"}")
    };

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is not null)
        {
            _lastRequestContent = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        return Response;
    }
}
