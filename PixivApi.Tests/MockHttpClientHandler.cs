using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PixivApi.Tests;

public class MockHttpClientHandler : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Method == HttpMethod.Get)
        {
            if (UrlPayloadMapping.Mapping.TryGetValue(request.RequestUri.AbsoluteUri, out var jsonFilename))
            {
                var jsonPayload = File.ReadAllText(Path.Join("Payloads", jsonFilename));
                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(jsonPayload),
                };
                response.Headers.Add("x-userid", "1000");
                return Task.FromResult<HttpResponseMessage>(response);
            }
            else
            {
                throw new KeyNotFoundException(
                    $"No corresponding payload was found for the request Uri {request.RequestUri.AbsoluteUri}");
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}