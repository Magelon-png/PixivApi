using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization.Metadata;
using System.Web;
using Polly;
using Scighost.PixivApi.Exceptions;
using Scighost.PixivApi.Helpers;
using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Fanbox;
using Scighost.PixivApi.SerializerContexts;

namespace Scighost.PixivApi.Clients;

/// <summary>
/// 
/// </summary>
public class FanboxClient : IDisposable
{
    private const string OriginUrl = "https://www.fanbox.cc";
    private const string ReferrerUrl = "https://www.fanbox.cc/";
    private const string BaseUriHttps = "https://api.fanbox.cc/";
    private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36 Edg/138.0.0.0";


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
    /// Internal HttpClient instance
    /// </summary>
    public HttpClient HttpClient => _httpClient;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cfBm">__cf_bm cookie value</param>
    /// <param name="cfClearance">cf_clearance cookie value</param>
    /// <param name="fanboxsessid">FANBOXSESSID cookie value</param>
    /// <param name="additionalCookies">Additional cookies to append</param>
    /// <param name="clientHandler"></param>
    /// <param name="userAgent"></param>
    /// <param name="enableCurlImpersonate"></param>
    /// <param name="curlImpersonatePath"></param>
    /// <exception cref="PixivException"></exception>
    public FanboxClient(string cfBm, string cfClearance, string fanboxsessid, Dictionary<string, string>? additionalCookies = null, HttpMessageHandler? clientHandler = null, string? userAgent = null, bool enableCurlImpersonate = false, string? curlImpersonatePath = null)
    {
        var cookie = $"__cf_bm={cfBm}; cf_clearance={cfClearance}; FANBOXSESSID={fanboxsessid};";
        if (additionalCookies != null)
        {
            foreach (var kvp in additionalCookies)
            {
                cookie += $" {kvp.Key}={kvp.Value};";
            }
        }

        if (ValidateCookie(cookie) == false)
        {
            throw new PixivException("Invalid cookie. The cookie should be in the format of '__cf_bm=xxx;cf_clearance=yyy;FANBOXSESSID=zzz;' in any order.");
        }
        _resiliencePipeline = HttpClientHelper.GetResiliencePipeline();
        
        var httpHandler = clientHandler ?? (HttpMessageHandler) (enableCurlImpersonate ? new CurlImpersonateHandler(curlImpersonatePath) : new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All });

        _httpClient = new HttpClient(httpHandler);
        _downloadHttpClient = new HttpClient(clientHandler ?? new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All });
        _httpClient.BaseAddress = new Uri(BaseUriHttps);
        
        _httpClient.DefaultRequestVersion = HttpVersion.Version20;
        _httpClient.DefaultRequestHeaders.Add("Origin", OriginUrl);
        _httpClient.DefaultRequestHeaders.Add("Priority", "u=1, i");
        _httpClient.DefaultRequestHeaders.Add("Referer", ReferrerUrl);
        _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent ?? DefaultUserAgent);


        _downloadHttpClient.DefaultRequestVersion = HttpVersion.Version20;        
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
            async token => await _httpClient.PostAsJsonAsync(url, value, FanboxJsonSerializerContext.Default.Object,token), cancellationToken);
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
    /// <param name="creatorId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<SupportingPlan[]> GetCreatorSupportingPlansAsync(string creatorId, CancellationToken cancellationToken = default)
    {
        var url = $"plan.listCreator?creatorId={creatorId}";
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
    /// <param name="searchTerm"></param>
    /// <param name="page">First page at 0</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreatorSearchResult> SearchCreatorsAsync(string searchTerm, int page = 0, CancellationToken cancellationToken = default)
    {
        var url = $"creator.search?q={Uri.EscapeDataString(searchTerm)}&page={page}";
        var response = await CommonGetAsync<CreatorSearchResult>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperCreatorSearchResult, cancellationToken);
        return response;
    }
    
    /// <summary>
    /// Get recommended creators
    /// </summary>
    /// <param name="limit">Optional limit for the number of creators</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Search result with recommended creators</returns>
    public async Task<SearchRecommendCreatorsResult> GetRecommendedCreatorsAsync(int? limit = null, CancellationToken cancellationToken = default)
    {
        var url = "creator.getRecommended";
        if(limit.HasValue)        {
            url += $"?limit={limit.Value}";
        }
        var response = await CommonGetAsync<SearchRecommendCreatorsResult>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperSearchRecommendCreatorsResult, cancellationToken);
        return response;
    }

    /// <summary>
    /// Get followed Pixiv creators
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Search result with followed Pixiv creators</returns>
    public async Task<SearchRecommendCreatorsResult> GetFollowedPixivCreators(
        CancellationToken cancellationToken = default)
    {
        var url = "creator.listPixiv";
        var response = await CommonGetAsync<SearchRecommendCreatorsResult>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperSearchRecommendCreatorsResult, cancellationToken);
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
    /// <param name="getSupportedCreatorsOnly">When false, returns both followed and supported creators' posts</param>
    /// <param name="nextUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HomePagePostItems> GetHomePagePostsAsync(bool getSupportedCreatorsOnly = false, string? nextUrl = null, CancellationToken cancellationToken = default)
    {
        var url = getSupportedCreatorsOnly ? "post.listSupporting" : "post.listHome";
        
        if(nextUrl != null)
        {
            url = nextUrl.Replace(BaseUriHttps, "");
        }
        else
        {
            url += "?limit=10";
        }
        return await CommonGetAsync<HomePagePostItems>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperHomePagePostItems, cancellationToken);
    }

    /// <summary>
    /// Get notifications
    /// </summary>
    /// <param name="nextUrl">URL for next page</param>
    /// <param name="commentOnly">Whether to get only comment notifications</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Notification result</returns>
    public async Task<GetNotificationResult> GetNotificationAsync(string? nextUrl = null, bool commentOnly = false,
        CancellationToken cancellationToken = default)
    {
        var url = "bell.list";
        
        if(nextUrl != null)
        {
            url = nextUrl.Replace(BaseUriHttps, "");
        }
        else
        {
            url += "?limit=10&skipConvertUnreadNotification=0";
            if (commentOnly)
            {
                url += "&commentOnly=1";
            }
        }
        return await CommonGetAsync<GetNotificationResult>(url, FanboxJsonSerializerContext.Default.FanboxResponseWrapperGetNotificationResult, cancellationToken);
    }

    /// <summary>
    /// Marks all newsletter notifications as read.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task MarkNewsletterNotificationsAsReadAsync(CancellationToken cancellationToken = default)
    {
        var url = "newsletter.markAsReadAll";
        
        await CommonPostAsync<EmptyResponse>(url, new EmptyResponse(),FanboxJsonSerializerContext.Default.FanboxResponseWrapperEmptyResponse, cancellationToken);
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
        var retries = 0;
        var maxRetries = 3;
        var downloaded = false;
        while (retries < maxRetries && !downloaded)
        {
            try
            {
                using var response = await _downloadHttpClient.GetAsync(fileUrl,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                response.EnsureSuccessStatusCode();
                await response.Content.CopyToAsync(destinationStream, cancellationToken);
                downloaded = true;
            }
            catch
            {
                await destinationStream.FlushAsync(cancellationToken);
                destinationStream.Seek(0, SeekOrigin.Begin);
                retries++;
            }
        }
        
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

/// <summary>
/// Empty response for API calls that don't return data
/// </summary>
public record EmptyResponse();
