using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using Polly;
using Scighost.PixivApi.Common;
using Scighost.PixivApi.Fanbox;

namespace Scighost.PixivApi;

/// <summary>
/// 
/// </summary>
public class FanboxClient : IDisposable
{
    private const string OriginUrl = "https://www.fanbox.cc";
    private const string ReferrerUrl = "https://www.fanbox.cc/";
    private const string BaseUriHttps = "https://api.fanbox.cc/";
    private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36 Edg/136.0.0.0";


    private readonly HttpClient _httpClient;
    private readonly HttpClient _downloadHttpClient;
    private ResiliencePipeline _resiliencePipeline;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cookie"></param>
    /// <returns></returns>
    public static bool ValidateCookie(string cookie)
    {
        if (string.IsNullOrWhiteSpace(cookie))
            return false;
        
        var requiredKeys = new[] { "__cf_bm", "cf_clearance", "FANBOXSESSID" };
    
        foreach (var key in requiredKeys)
        {
            if (!cookie.Contains(key + "="))
                return false;
        }
    
        return true;
    }


    /// <summary>
    /// 内部的 HttpClient 实例
    /// </summary>
    public HttpClient HttpClient => _httpClient;
    
    public class ForceHttp2Handler : DelegatingHandler
    {
        public ForceHttp2Handler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = HttpVersion.Version20;
            return base.SendAsync(request, cancellationToken);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cookie"></param>
    /// <param name="clientHandler"></param>
    /// <exception cref="PixivException"></exception>
    public FanboxClient(string cookie, HttpClientHandler? clientHandler = null)
    {
        if (ValidateCookie(cookie) == false)
        {
            throw new PixivException("Invalid cookie. The cookie should be in the format of '__cf_bm=xxx;cf_clearance=yyy;FANBOXSESSID=zzz;' in any order.");
        }
        _resiliencePipeline = HttpClientHelper.GetResiliencePipeline();

        _httpClient = new HttpClient(new ForceHttp2Handler(clientHandler ?? new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }));
        _downloadHttpClient = new HttpClient(new ForceHttp2Handler( clientHandler ?? new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }));
        _httpClient.BaseAddress = new Uri(BaseUriHttps);
        
        
        _httpClient.DefaultRequestHeaders.Add("Origin", OriginUrl);
        _httpClient.DefaultRequestHeaders.Add("Priority", "u=1, i");
        _httpClient.DefaultRequestHeaders.Add("Referer", ReferrerUrl);
        _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);


        
        _downloadHttpClient.DefaultRequestHeaders.Add("Origin", OriginUrl);
        _downloadHttpClient.DefaultRequestHeaders.Add("Priority", "u=1, i");
        _downloadHttpClient.DefaultRequestHeaders.Add("Cookie", cookie);
        _downloadHttpClient.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);
        _downloadHttpClient.DefaultRequestHeaders.Add("Referer", ReferrerUrl);
    }
    
    private async Task<T> CommonGetAsync<T>(string url, JsonTypeInfo<FanboxResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var wrapper = await _resiliencePipeline.ExecuteAsync(
            async token => await _httpClient.GetFromJsonAsync<FanboxResponseWrapper<T>>(url, jsonTypeInfo, token),
            cancellationToken);
        if (wrapper is null)
        {
            throw new PixivException("Error");
        }
        return wrapper.Body;
    }


    private async Task<T> CommonPostAsync<T>(string url, object value, JsonTypeInfo<FanboxResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var response =  await _resiliencePipeline.ExecuteAsync(
            async token => await _httpClient.PostAsJsonAsync(url, value, token), cancellationToken);
        response.EnsureSuccessStatusCode();
        var wrapper = await response.Content.ReadFromJsonAsync<FanboxResponseWrapper<T>>(jsonTypeInfo, cancellationToken);
        if (wrapper is null)
        {
            throw new PixivException("Error");
        }
        return wrapper.Body;
    }


    private async Task<T> CommonSendAsync<T>(HttpRequestMessage message, JsonTypeInfo<FanboxResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var response =  await _resiliencePipeline.ExecuteAsync(
            async token => await _httpClient.SendAsync(message, token), cancellationToken);
        response.EnsureSuccessStatusCode();
        var wrapper = await response.Content.ReadFromJsonAsync<FanboxResponseWrapper<T>>(jsonTypeInfo, cancellationToken);
        if (wrapper is null)
        {
            throw new PixivException("Error");
        }
        return wrapper.Body;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<SupportingPlan[]> GetSupportingPlansAsync(CancellationToken cancellationToken = default)
    {
        var url = "plan.listSupporting";
        var response = await CommonGetAsync<SupportingPlan[]>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperSupportingPlanArray, cancellationToken);
        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FollowedCreator[]> GetFollowedCreatorsAsync(CancellationToken cancellationToken = default)
    {
        var url = "creator.listFollowing";
        var response = await CommonGetAsync<FollowedCreator[]>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperFollowedCreatorArray, cancellationToken);
        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="creatorId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string[]> GetCreatorPostPaginationAsync(string creatorId, CancellationToken cancellationToken = default)
    {
        var url = $"post.paginateCreator?creatorId={creatorId}";
        var response = await CommonGetAsync<string[]>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperStringArray, cancellationToken);
        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PostListItem[]> GetCreatorPostsFromPaginationAsync(string paginationUrl, CancellationToken cancellationToken = default)
    {
        var url = paginationUrl.Replace(BaseUriHttps, "");
        var response = await CommonGetAsync<PostListItem[]>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperPostListItemArray, cancellationToken);
        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PostInfo> GetPostInfoAsync(int postId, CancellationToken cancellationToken = default)
    {
        var url = $"post.info?postId={postId}";
        var response = await CommonGetAsync<PostInfo>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperPostInfo, cancellationToken);
        return response;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Stream> DownloadFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var response = await _downloadHttpClient.GetAsync(fileUrl, cancellationToken);
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        return content;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <param name="destinationStream"></param>
    /// <param name="cancellationToken"></param>
    public async Task DownloadFileAsync(string fileUrl, Stream destinationStream, CancellationToken cancellationToken = default)
    {
        using var response = await _downloadHttpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        await response.Content.CopyToAsync(destinationStream, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="PixivClient"/> instance.
    /// Disposes the internal HTTP clients utilized for network communication.
    /// </summary>
    public virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _httpClient.Dispose();
        _downloadHttpClient.Dispose();

    }
}