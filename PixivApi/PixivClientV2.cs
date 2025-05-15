using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using System.Web;
using NetEscapades.EnumGenerators;
using Polly;
using Scighost.PixivApi.Search;
using Scighost.PixivApi.V2;
using Scighost.PixivApi.V2.Illust;
using IllustInfoResponse = Scighost.PixivApi.V2.Illust.IllustInfoResponse;

namespace Scighost.PixivApi;

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
    /// <param name="cookie">Cookie to authenticate with the API</param>
    /// <param name="clientHandler">Custom HTTP client handler for testing or other cases</param>
    public PixivClientV2(HttpClientHandler? clientHandler = null)
    {
        _codeGenerator = new PkceCodeGenerator();
        _resiliencePipeline = HttpClientHelper.GetResiliencePipeline();
        _httpClient = new HttpClient(clientHandler ?? new HttpClientHandler 
            { AutomaticDecompression = DecompressionMethods.All });
        _httpClient.BaseAddress = new Uri(BaseUriHttps);

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
    
    internal partial class PkceCodeGenerator
    {

        internal string CodeVerifier;

        internal string CodeChallenge;

        internal PkceCodeGenerator()
        {
            Regenerate();
        }
        
        internal void Regenerate()
        {
            CodeVerifier = GenerateNonce();
            CodeChallenge = GenerateCodeChallenge(CodeVerifier);
        }

        private string GenerateNonce()
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

        var response = await _resiliencePipeline.ExecuteAsync(
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
            var responseData = await JsonSerializer.DeserializeAsync<OauthLoginResponse>(await response.Content.ReadAsStreamAsync());
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
            var responseData = await JsonSerializer.DeserializeAsync<OauthLoginResponse>(await response.Content.ReadAsStreamAsync());
            _refreshToken = responseData?.RefreshToken ?? throw new ArgumentNullException(null, "Refresh token is null");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {responseData.AccessToken}");
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
    /// <returns>
    /// The retrieved illustrations starting from the offset. For additional illustrations, retrieve the expected offset from the "NextUrl" property.
    /// </returns>
    public async Task<IllustsInfoResponse> GetUserIllustsAsync(uint userId, int offset = 0, IllustType illustType = IllustType.Illust, CancellationToken cancellationToken = default)
    {
        var url = $"/v1/user/illusts?user_id={userId}&offset={offset}&type={illustType.ToStringFast(true)}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
    }

    [EnumExtensions]
    public enum IllustType
    {
        [Display(Name = "illust")]
        Illust,
        [Display(Name = "manga")]
        Manga,
        [Display(Name = "illust_manga")]
        IllustManga,
        [Display(Name = "novel")]
        Novel
    }
    
    public async Task<IllustInfoResponse> GetIllustDetailsAsync(uint illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/v1/illust/detail?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustInfoResponse, cancellationToken);
    }

    public async Task<UgoiraMetadataResponse> GetUgoiraMetadataAsync(uint illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/v1/ugoira/metadata?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.UgoiraMetadataResponse, cancellationToken);
    }

    public async Task<IllustCommentsResponse> GetIllustCommentsAsync(uint illustId,
        CancellationToken cancellationToken = default)
    {
        var url = $"/v1/illust/comments?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustCommentsResponse, cancellationToken);
    }

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

    public async Task<IllustsInfoResponse> GetRelatedIllusts(uint illustId,
        CancellationToken cancellationToken = default)
    {
        var url = $"v1/illust/related?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustsInfoResponse, cancellationToken);
    }

    public async Task<BookmarkDetailResponse> GetIllustBookmarkDetailAsync(uint illustId, CancellationToken cancellationToken = default)
    {
        var url = $"v1/illust/bookmark/detail?illust_id={illustId}";
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.BookmarkDetailResponse, cancellationToken);
    }

    public async Task<IllustInfoResponse> GetMyIllustrationsAsync(string? nextUrl = null,
        CancellationToken cancellationToken = default)
    {
        const string url = "/v2/illust/mypixiv";

        return await CommonGetAsync(nextUrl ?? url, PixivV2JsonSerializerContext.Default.IllustInfoResponse, cancellationToken);
    }
    
    public async Task<IllustInfoResponse> GetNewIllustrationsAsync(IllustrationContentType illustContentType, string? nextUrl = null,
        CancellationToken cancellationToken = default)
    {
        var url = nextUrl ?? "/v1/illust/new";
        url += $"?content_type={illustContentType.ToStringFast(true)}";

        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustInfoResponse, cancellationToken);
    }
    
    public async Task<IllustInfoResponse> GetPopularIllustrationsAsync(IllustrationContentType illustContentType, string? nextUrl = null,
        CancellationToken cancellationToken = default)
    {
        var url = nextUrl ?? "/v1/illust/popular";
        url += $"?content_type={illustContentType.ToStringFast(true)}";

        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustInfoResponse, cancellationToken);
    }

    public async Task<RecommendedIllustResponse> GetRecommendedIllustrations(IllustrationContentType illustContentType,
        int offset = 0,
        bool includeRanking = false, bool includeRankingLabel = false, uint? MinBookmarkIdForRecentIllust = null,
        uint? MaxBookmarkIdForRecommendd = null, string? nextUrl = null, CancellationToken cancellationToken = default)
    {
        if (nextUrl is not null)
        {
            return await CommonGetAsync(nextUrl, PixivV2JsonSerializerContext.Default.RecommendedIllustResponse, cancellationToken);
        }

        var url = "/v1/illust/recommended";
        var queryString = HttpUtility.ParseQueryString(String.Empty);
        queryString["content_type"] = illustContentType.ToStringFast(true);
        queryString["include_ranking"] = includeRanking.ToString(CultureInfo.InvariantCulture);
        queryString["include_ranking_label"] = includeRankingLabel.ToString(CultureInfo.InvariantCulture);
        if (MinBookmarkIdForRecentIllust is not null)
        {
            queryString["min_bookmark_id_for_recent_illust"] = MinBookmarkIdForRecentIllust.Value.ToString(NumberFormatInfo.InvariantInfo);
        }
        if (MaxBookmarkIdForRecommendd is not null)
        {
            queryString["max_bookmark_id_for_recommend"] = MaxBookmarkIdForRecommendd.Value.ToString(NumberFormatInfo.InvariantInfo);
        }
        queryString["offset"] = offset.ToString(NumberFormatInfo.InvariantInfo);
        return await CommonGetAsync($"{url}?{queryString}", PixivV2JsonSerializerContext.Default.RecommendedIllustResponse, cancellationToken);
    }

    public async Task<IllustSearchResponse> SearchIllustsAsync(string searchTerm, SearchOrderV2 orderBy,
        SearchTarget searchTarget = SearchTarget.PartialMatchForTags, BookmarkCount? bookmarkCount = null,
        SearchPeriod? searchPeriod = null, string? nextUrl = null, uint? offset = null, CancellationToken cancellationToken = default)
    {
        if (nextUrl is not null)
        {
            return await CommonGetAsync(nextUrl, PixivV2JsonSerializerContext.Default.IllustSearchResponse, cancellationToken);
        }
        
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
        
        return await CommonGetAsync(url, PixivV2JsonSerializerContext.Default.IllustSearchResponse, cancellationToken);
    }

    [EnumExtensions]
    public enum SearchOrderV2
    {
    [Display(Name = "date_desc")]
    DateDescending,
    [Display(Name = "date_asc")]
    DateAscending,
    [Display(Name = "popular_desc")]
    PopularDescending
    }

    [EnumExtensions]
    public enum SearchTarget
    {
        [Display(Name = "partial_match_for_tags")]
        PartialMatchForTags,
        [Display(Name = "exact_match_for_tags")]
        ExactMatchForTags,
        [Display(Name = "title_and_caption")]
        TitleAndCaption,
        [Display(Name = "text")]
        Text,
        [Display(Name = "keyword")]
        Keyword
    }

    [EnumExtensions]
    public enum BookmarkCount
    {
        [Display(Name = "0")]
        None = 0,
        [Display(Name = "10")]
        Ten = 1,
        [Display(Name = "30")]
        Thirty = 2,
        [Display(Name = "50")]
        Fifty = 3,
        [Display(Name = "100")]
        Hundred = 4,
        [Display(Name = "300")]
        ThreeHundred = 5,
        [Display(Name = "500")]
        FiveHundred = 6,
        [Display(Name = "1000")]
        Thousand = 7,
        [Display(Name = "5000")]
        FiveThousand = 8
    }

    [EnumExtensions]
    public enum SearchPeriod
    {
        [Display(Name = "within_last_day")]
        LastDay,
        [Display(Name = "within_last_week")]
        LastWeek,
        [Display(Name = "within_last_month")]
        LastMonth
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