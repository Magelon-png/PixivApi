using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using System.Web;
using Polly;
using Scighost.PixivApi.Exceptions;
using Scighost.PixivApi.Helpers;
using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Novel;
using Scighost.PixivApi.Models.V2.Common;
using Scighost.PixivApi.Models.V2.Illust;
using Scighost.PixivApi.Models.V2.Novel;
using Scighost.PixivApi.Models.V2.User;
using Scighost.PixivApi.SerializerContexts;
using NovelInfoResponse = Scighost.PixivApi.Models.V2.Novel.NovelInfoResponse;

namespace Scighost.PixivApi.Clients;

/// <summary>
/// 
/// </summary>
public partial class PixivClientV2 : IDisposable
{
    private const string BaseUriHttps = "https://app-api.pixiv.net";
    private const string DefaultUserAgent = "PixivAndroidApp/6.66.1 (Android 11; Pixel 5)";
    private const string AuthTokenUrl = "https://oauth.secure.pixiv.net/auth/token";
    private const string LoginUrl = "https://app-api.pixiv.net/web/v1/login";
    private const string ClientId = "MOBrBDS8blbauoSck0ZfDbtuzpyT";
    private const string ClientSecret = "lsACyCD94FhDUtGTXi3QzcFE2uU1hqtDaKeqrdwj";
    private const string RedirectUri = "https://app-api.pixiv.net/web/v1/users/auth/pixiv/callback";

    private readonly HttpClient _httpClient;
    private readonly HttpClient _downloadHttpClient;
    private readonly PkceCodeGenerator _codeGenerator;
    private readonly ResiliencePipeline _resiliencePipeline;
    
    private string _refreshToken = string.Empty;
    
    private DateTimeOffset _tokenExpiresAt = DateTimeOffset.MaxValue;
    
    /// <summary>
    /// HttpClient usable for not yet supported endpoints
    /// </summary>
    public HttpClient HttpClient => _httpClient;
    
    /// <summary>
    /// Pixiv client allowing to interact with the Pixiv API
    /// </summary>
    /// <param name="clientHandler">Custom HTTP client handler for testing or other cases</param>
    public PixivClientV2(HttpClientHandler? clientHandler = null)
    {
        _codeGenerator = new PkceCodeGenerator();
        _resiliencePipeline = HttpClientHelper.GetResiliencePipeline();
        _httpClient = new HttpClient(clientHandler ?? new HttpClientHandler 
            { AutomaticDecompression = DecompressionMethods.All });
        _httpClient.BaseAddress = new Uri(BaseUriHttps);

        _httpClient.DefaultRequestVersion = HttpVersion.Version30;
        _httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        _httpClient.DefaultRequestHeaders.Add("Priority", "u=1, i");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);
        _httpClient.DefaultRequestHeaders.Add("Referer", BaseUriHttps);

        _downloadHttpClient = new HttpClient(clientHandler ?? new HttpClientHandler
            { AutomaticDecompression = DecompressionMethods.All })
        {
            DefaultRequestHeaders =
            {
                {
                    "User-Agent", DefaultUserAgent
                }
            }
        };
    }
    
    internal sealed partial class PkceCodeGenerator
    {

        internal string? CodeVerifier;

        internal string? CodeChallenge;

        internal PkceCodeGenerator()
        {
            Regenerate();
        }
        
        internal void Regenerate()
        {
            CodeVerifier = PkceCodeGenerator.GenerateNonce();
            CodeChallenge = GenerateCodeChallenge(CodeVerifier);
        }

        private static string GenerateNonce()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            var random = new Random();
            var nonce = new char[32];
            for (var i = 0; i < nonce.Length; i++)
            {
                nonce[i] = chars[random.Next(chars.Length)];
            }

            return new string(nonce);
        }

        private string GenerateCodeChallenge(string codeVerifier)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = MyRegex1().Replace(code, "_");
            code = MyRegex().Replace(code, "");
            return code;
        }

        [GeneratedRegex("=+$")]
        private partial Regex MyRegex();
        [GeneratedRegex("\\/")]
        private partial Regex MyRegex1();
    }
    
    #region Common Method


    private async Task<T> CommonGetAsync<T>(string url, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        if (_tokenExpiresAt.AddMinutes(-10) < DateTimeOffset.Now)
        {
            await RefreshTokenAsync();
        }

        var response = await _resiliencePipeline.ExecuteAsync(async token => await _httpClient.GetAsync(url, token),
            cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync(
                PixivV2JsonSerializerContext.Default.PixivV2ErrorWrapper, cancellationToken);
            throw new PixivException(error?.Error?.Message ?? "");
        }
#if DEBUG
    var stringContent = await response.Content.ReadAsStringAsync(cancellationToken);
    Trace.WriteLine($"Response content for {url}: {stringContent}");
#endif
        var responseContent = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
        if (responseContent is null)
        {
            throw new InvalidCastException("Unable to convert the response to a corresponding type");
        }
        return responseContent;
    }


    private async Task<T> CommonPostAsync<T>(string url, object value, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        if (_tokenExpiresAt.AddMinutes(-10) < DateTimeOffset.Now)
        {
            await RefreshTokenAsync();
        }

        var response = await _resiliencePipeline.ExecuteAsync<HttpResponseMessage>(
            async token =>
                await _httpClient.PostAsJsonAsync(url, value, PixivV2JsonSerializerContext.Default.Object, token),
            cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync(
                PixivV2JsonSerializerContext.Default.PixivV2ErrorWrapper, cancellationToken);
            throw new PixivException(error?.Error?.Message ?? "");
        }
        var responseContent = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
        if (responseContent is null)
        {
            throw new InvalidCastException("Unable to convert the response to a corresponding type");
        }
        return responseContent;
    }


    private async Task CommonSendAsync<T>(HttpRequestMessage message, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        if (_tokenExpiresAt.AddMinutes(-10) < DateTimeOffset.Now)
        {
            await RefreshTokenAsync();
        }
        var response =  await _resiliencePipeline.ExecuteAsync(
            async token => await _httpClient.SendAsync(message, token), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync(
                PixivV2JsonSerializerContext.Default.PixivV2ErrorWrapper, cancellationToken);
            throw new PixivException(error?.Error?.Message ?? "");
        }
        var responseContent = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
        if (responseContent is null)
        {
            throw new InvalidCastException("Unable to convert the response to a corresponding type");
        }
    }

    /// <summary>
    /// Retrieves the access token and refresh token necessary to contact the APIs. Should be called before any other method other than RefreshTokenAsync.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<bool> LoginAsync(string code)
    {
        var data = new OauthLoginBody(ClientId, ClientSecret, code, 
            _codeGenerator.CodeVerifier, GrantType.AuthorizationCode, RedirectUri, null);


        using HttpContent content = new FormUrlEncodedContent(data.ToDictionary()) ;
        
        var response = await _downloadHttpClient.PostAsync(AuthTokenUrl, content);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseData = await JsonSerializer.DeserializeAsync<OauthLoginResponse>(await response.Content.ReadAsStreamAsync(), PixivV2JsonSerializerContext.Default.OauthLoginResponse);
            _refreshToken = responseData?.RefreshToken ?? throw new ArgumentNullException(null, "Refresh token is null");
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {responseData.AccessToken}");
            _tokenExpiresAt = DateTimeOffset.Now.AddSeconds(responseData.ExpiresIn);
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Refresh the access token and refresh token. This is called automatically to refresh the access token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public async Task<bool> RefreshTokenAsync(string? refreshToken = null)
    {
        var data = new OauthLoginBody(ClientId, ClientSecret, _codeGenerator.CodeVerifier, 
            _codeGenerator.CodeVerifier, GrantType.RefreshToken, RedirectUri, refreshToken ?? _refreshToken);
        
        using HttpContent content = new FormUrlEncodedContent(data.ToDictionary()) ;
        
        var response = await _downloadHttpClient.PostAsync(AuthTokenUrl, content);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseData = await JsonSerializer.DeserializeAsync<OauthLoginResponse>(await response.Content.ReadAsStreamAsync(), PixivV2JsonSerializerContext.Default.OauthLoginResponse);
            _refreshToken = responseData?.RefreshToken ?? throw new ArgumentNullException(null, "Refresh token is null");
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.AccessToken);
            _tokenExpiresAt = DateTimeOffset.Now.AddSeconds(responseData.ExpiresIn);
            return true;
        }

        return false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetCodeLoginUrl()
    {
        _codeGenerator.Regenerate();
        var queryStringBuilder = HttpUtility.ParseQueryString(String.Empty);
        queryStringBuilder.Add("client", "pixiv-android");
        queryStringBuilder.Add("code_challenge", _codeGenerator.CodeChallenge);
        queryStringBuilder.Add("code_challenge_method", "S256");
        return $"{LoginUrl}?{queryStringBuilder}";
    }

    #endregion

    

    #region Illusts

    /// <summary>
    /// Gets the illustrations of a user.
    /// </summary>
    /// <param name="userId">Id of the user</param>
    /// <param name="offset">Offset from the newest illustration</param>
    /// <param name="illustType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// The retrieved illustrations starting from the offset. For additional illustrations, retrieve the expected offset from the "NextUrl" property.
    /// </returns>
    public async Task<IllustsInfoResponse> GetUserIllustsAsync(int userId, int offset = 0, IllustType illustType = IllustType.Illust, CancellationToken cancellationToken = default)
    {
        var url = $"/v1/user/illusts?user_id={userId}&offset={offset}&type={illustType.ToStringFast(true)}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IllustInfoResponse> GetIllustDetailsAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/v1/illust/detail?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustInfoResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UgoiraMetadataResponse> GetUgoiraMetadataAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/v1/ugoira/metadata?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.UgoiraMetadataResponse, cancellationToken);
    }
    // Endpoint no longer exists
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="illustId"></param>
    // /// <param name="cancellationToken"></param>
    // /// <returns></returns>
    // public async Task<IllustCommentsResponse> GetIllustCommentsAsync(int illustId,
    //     CancellationToken cancellationToken = default)
    // {
    //     var url = $"/v1/illust/comments?illust_id={illustId}";
    //     
    //     return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustCommentsResponse, cancellationToken);
    // }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rankingMode"></param>
    /// <param name="date"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IllustsInfoResponse> GetIllustsRanking(RankingMode rankingMode, DateTimeOffset? date = null, 
        CancellationToken cancellationToken = default)
    {
        var url = $"v1/illust/ranking?mode={rankingMode.ToStringFast(true)}";
        if (date is not null)
        {
            url += $"&date={date?.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)
                            ?? DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}";
        }
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IllustsInfoResponse> GetRelatedIllusts(int illustId,
        CancellationToken cancellationToken = default)
    {
        var url = $"v2/illust/related?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="restrict"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<IllustInfoV2> GetFollowIllustrationsAsync(string? restrict = "public",
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"/v2/illust/follow?restrict={restrict}";
        
        IllustsInfoResponse? response = null;

        do
        {
            response =  await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
            foreach (var illust in response.Illusts)
            {
                yield return illust;
            }
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BookmarkDetailResponse> GetIllustBookmarkDetailAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"v2/illust/bookmark/detail?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.BookmarkDetailResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<IllustInfoV2> GetMyIllustrationsAsync( 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string url = "/v2/illust/mypixiv";

        IllustsInfoResponse? response = null;

        do
        {
            response =  await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
            foreach (var illust in response.Illusts)
                yield return illust;
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustContentType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<IllustInfoV2> GetNewIllustrationAsync(IllustrationContentType illustContentType,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/illust/new";
        url += $"?content_type={illustContentType.ToStringFast(true)}";
        
        IllustsInfoResponse? response = null;

        do
        {
            response =  await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
            foreach (var illust in response.Illusts)
                yield return illust;
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }
    // Endpoint does not exists
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="illustContentType"></param>
    // /// <param name="nextUrl"></param>
    // /// <param name="cancellationToken"></param>
    // /// <returns></returns>
    // public async Task<IllustsInfoResponse> GetPopularIllustrationAsync(IllustrationContentType illustContentType, string? nextUrl = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     var url = nextUrl ?? "/v1/illust/popular";
    //     url += $"?content_type={illustContentType.ToStringFast(true)}";
    //
    //     return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
    // }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustContentType"></param>
    /// <param name="offset"></param>
    /// <param name="includeRanking"></param>
    /// <param name="includeRankingLabel"></param>
    /// <param name="minBookmarkIdForRecentIllust"></param>
    /// <param name="maxBookmarkIdForRecommendd"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<RecommendedIllustResponse> GetRecommendedIllustrations(IllustrationContentType illustContentType,
        int offset = 0,
        bool includeRanking = false, bool includeRankingLabel = false, int? minBookmarkIdForRecentIllust = null,
        int? maxBookmarkIdForRecommendd = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {

        var url = "/v1/illust/recommended";
        var queryString = HttpUtility.ParseQueryString(String.Empty);
        queryString["content_type"] = illustContentType.ToStringFast(true);
        queryString["include_ranking"] = includeRanking.ToString(CultureInfo.InvariantCulture);
        queryString["include_ranking_label"] = includeRankingLabel.ToString(CultureInfo.InvariantCulture);
        if (minBookmarkIdForRecentIllust is not null)
        {
            queryString["min_bookmark_id_for_recent_illust"] = minBookmarkIdForRecentIllust.Value.ToString(NumberFormatInfo.InvariantInfo);
        }
        if (maxBookmarkIdForRecommendd is not null)
        {
            queryString["max_bookmark_id_for_recommend"] = maxBookmarkIdForRecommendd.Value.ToString(NumberFormatInfo.InvariantInfo);
        }
        queryString["offset"] = offset.ToString(NumberFormatInfo.InvariantInfo);
        
        RecommendedIllustResponse? response = null;
        do
        {
            response = await CommonGetAsync($"{url}?{queryString}", PixivV2JsonSerializerContext.Default.RecommendedIllustResponse, cancellationToken);
            yield return response;
            url = response.NextUrl;
        } while(!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustContentType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<RecommendedIllustResponse> GetRecommendedIllustrationsNoLoginAsync(IllustrationContentType illustContentType,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"/v1/illust/recommended-nologin?content_type={illustContentType.ToStringFast(true)}";
        
        RecommendedIllustResponse? response = null;
        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.RecommendedIllustResponse, cancellationToken);
            yield return response;
            url = response.NextUrl;
        } while(!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <param name="orderBy"></param>
    /// <param name="searchTarget"></param>
    /// <param name="bookmarkCount"></param>
    /// <param name="searchPeriod"></param>
    /// <param name="offset"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<IllustInfoV2> SearchIllustsAsync(string searchTerm, SearchOrderV2 orderBy,
        SearchTarget searchTarget = SearchTarget.PartialMatchForTags, BookmarkCount? bookmarkCount = null,
        SearchPeriod? searchPeriod = null, int? offset = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/search/illust";
        var queryString = HttpUtility.ParseQueryString(String.Empty);
        queryString["word"] = searchTerm;
        queryString["sort"] = orderBy.ToStringFast(true);
        queryString["search_target"] = searchTarget.ToStringFast(true);
        if (bookmarkCount is not null)
        {
            queryString["bookmark_num"] = bookmarkCount.Value.ToStringFast(true);
        }

        if (searchPeriod is not null)
        {
            queryString["duration"] = searchPeriod.Value.ToStringFast(true);
        }

        if (offset is not null)
        {
            queryString["offset"] = offset.Value.ToString(NumberFormatInfo.InvariantInfo);
        }
        
        url = url + "?" + queryString;
        
        IllustSearchResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustSearchResponse, cancellationToken);
            foreach (var illust in response.Illusts)
            {
                yield return illust;
            }
            url = response.NextUrl;
        } while(!string.IsNullOrWhiteSpace(url));
    }

    #endregion

    #region Novels

    /// <summary>
    /// 
    /// </summary>
    /// <param name="restrict"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<NovelProfile> GetFollowNovelsAsync(string restrict = "public", 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"/v1/novel/follow?restrict={restrict}";

        NovelsInfoResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.NovelsInfoResponse, cancellationToken);
            foreach (var novel in response.Novels)
            {
                yield return novel;
            }
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<NovelProfile> GetRecommendedNovelsNoLoginAsync(
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/novel/recommended-nologin";
        
        RecommendedNovelResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.RecommendedNovelResponse, cancellationToken);
            foreach (var novel in response.Novels)
            {
                yield return novel;
            }
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<NovelProfile> GetMyNovelsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/novel/mypixiv";
        
        NovelsInfoResponse? response = null;
        
        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.NovelsInfoResponse, cancellationToken);
            
            foreach (var novel in response.Novels)
            {
                yield return novel;
            }
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<NovelProfile> GetNewNovelsAsync(
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/novel/new";
        
        NovelsInfoResponse? response = null;
        
        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.NovelsInfoResponse, cancellationToken);
            
            foreach (var novel in response.Novels)
            {
                yield return novel;
            }
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="novelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<NovelInfoResponse> GetNovelDetailAsync(int novelId,
        CancellationToken cancellationToken = default)
    {
        var url = $"/v2/novel/detail?novel_id={novelId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.NovelInfoResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="novelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<IllustComment> GetNovelCommentsAsync(int novelId,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"/v1/novel/comments?novel_id={novelId}";
        
        IllustCommentsResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustCommentsResponse, cancellationToken);
            foreach (var comment in response.Comments)
            {
                yield return comment;
            }
            
            url = response.NextUrl;
        }
        while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="date"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<NovelProfile> GetNovelRankingAsync(RankingModeNovel mode, string? date = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"/v1/novel/ranking?mode={mode.ToStringFast(true)}";
        if (date is not null)
        {
            url += $"&date={date}";
        }

        NovelsInfoResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.NovelsInfoResponse,
                cancellationToken);
            foreach (var novel in response.Novels)
            {
                yield return novel;
            }

            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="novelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BookmarkDetailResponse> GetNovelBookmarkDetailAsync(int novelId,
        CancellationToken cancellationToken = default)
    {
        var url = $"/v2/novel/bookmark/detail?novel_id={novelId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.BookmarkDetailResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<NovelProfile> GetNovelMarkersAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/novel/markers";
        
        NovelsInfoResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.NovelsInfoResponse, cancellationToken);
            foreach (var novel in response.Novels)
            {
                yield return novel;
            }
            url = response.NextUrl;
        } while (!string.IsNullOrWhiteSpace(url));
        
    }

    #endregion

    #region Users

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UserFollowDetailResponse> GetUserFollowDetailAsync(int userId,
        CancellationToken cancellationToken = default)
    {
        var url = $"/v1/user/follow/detail?user_id={userId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.UserFollowDetailResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="restrict"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<BookmarkTag> GetUserBookmarkTagsIllustAsync(int userId, string restrict = "public", 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"/v1/user/bookmark-tags/illust?user_id={userId}&restrict={restrict}";
        
        UserBookmarkTagsResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.UserBookmarkTagsResponse, cancellationToken);
            foreach (var tag in response.BookmarkTags)
            {
                yield return tag;
            }
            url = response.NextUrl;
        }
        while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="restrict"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<BookmarkTag> GetUserBookmarkTagsNovelAsync(int userId, string restrict = "public",
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"/v1/user/bookmark-tags/novel?user_id={userId}&restrict={restrict}";
        
        UserBookmarkTagsResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.UserBookmarkTagsResponse, cancellationToken);
            foreach (var tag in response.BookmarkTags)
            {
                yield return tag;
            }
            url = response.NextUrl;
        }
        while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<IllustInfoV2> GetUserBrowsingHistoryIllustsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/user/browsing-history/illusts";
        
        IllustsInfoResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
            foreach (var illust in response.Illusts)
            {
                yield return illust;
            }
            url = response.NextUrl;
        }
        while (!string.IsNullOrWhiteSpace(url));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<NovelProfile> GetUserBrowsingHistoryNovelsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = "/v1/user/browsing-history/novels";
        
        NovelsInfoResponse? response = null;

        do
        {
            response = await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.NovelsInfoResponse, cancellationToken);
            foreach (var novel in response.Novels)
            {
                yield return novel;
            }
            url = response.NextUrl;
        }
        while (!string.IsNullOrWhiteSpace(url));
    }

    #endregion

    #region Miscellaneous

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TrendingTagsResponse> GetTrendingTagsIllustAsync(CancellationToken cancellationToken = default)
    {
        var url = "/v1/trending-tags/illust";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.TrendingTagsResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TrendingTagsResponse> GetTrendingTagsNovelAsync(CancellationToken cancellationToken = default)
    {
        var url = "/v1/trending-tags/novel";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.TrendingTagsResponse, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<object> GetEmojisAsync(CancellationToken cancellationToken = default)
    {
        var url = "/v1/emoji";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.Object, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<object> GetApplicationInfoAsync(CancellationToken cancellationToken = default)
    {
        var url = "/v1/application/info";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.Object, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<WalkthroughResponse> GetWalkthroughListAsync(CancellationToken cancellationToken = default)
    {
        var url = "/v1/walkthrough/list";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.WalkthroughResponse, cancellationToken);
    }

    #endregion

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the resources used by the PixivClientV2 instance.
    /// </summary>
    public virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _httpClient.Dispose();
        _downloadHttpClient.Dispose();
    }
}