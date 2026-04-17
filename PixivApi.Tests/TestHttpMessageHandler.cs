namespace PixivApi.Tests;

/// <summary>
/// Minimal HTTP handler for unit tests. Register expected request/response pairs
/// with <see cref="When(string,Func{HttpResponseMessage})"/> (GET) or
/// <see cref="When(HttpMethod,string,Func{HttpResponseMessage})"/> (any method)
/// before invoking the client under test.
/// </summary>
public sealed class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<(HttpMethod Method, string Uri), Func<HttpResponseMessage>> _responses = new();

    public void When(string url, Func<HttpResponseMessage> responseFactory)
        => _responses[(HttpMethod.Get, url)] = responseFactory;

    public void When(HttpMethod method, string url, Func<HttpResponseMessage> responseFactory)
        => _responses[(method, url)] = responseFactory;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        var key = (request.Method, request.RequestUri!.AbsoluteUri);
        if (_responses.TryGetValue(key, out var factory))
            return Task.FromResult(factory());

        throw new KeyNotFoundException(
            $"No response configured for {request.Method} {request.RequestUri!.AbsoluteUri}");
    }

    public HttpRequestMessage? LastRequest { get; private set; }
}
