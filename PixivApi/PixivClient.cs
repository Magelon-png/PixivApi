using System.Globalization;
using Scighost.PixivApi.Common;
using Scighost.PixivApi.Search;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using System.Web;

namespace Scighost.PixivApi;


/// <summary>
/// Request class for Pixiv API, where all requests are sent. When an error content is returned, a <see cref="PixivException"/> will be thrown.
/// <para />
/// When constructing an instance of this class, you can choose different constructors, with corresponding features like HTTP proxy, bypassing SNI blocking and specifying IP.
/// <para />
/// Pixiv's login process uses Cloudflare protection which is basically impossible to bypass. For features requiring an account, please log in through a browser and use the constructor with cookie and ua.
/// <para />
/// For non-GET operations like following and bookmarking, you need to first call <see cref="GetTokenAsync"/> to get a token. A return value of true indicates successful token acquisition. It is recommended to call this immediately after construction.
/// </summary>
public class PixivClient : IDisposable
{
    
    private const string BaseUriHttps = "https://www.pixiv.net/";
    private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36 Edg/135.0.0.0";


    private readonly HttpClient _httpClient;
    private readonly HttpClient _downloadHttpClient = new HttpClient()
    {
        DefaultRequestHeaders =
        {
            {
                "User-Agent", DefaultUserAgent
            },
            {
                "Referer", BaseUriHttps
            }
        }
    };

    /// <summary>
    /// HttpClient usable for not yet supported endpoints
    /// </summary>
    public HttpClient HttpClient => _httpClient;


    private string? _token;
    
    /// <summary>
    /// Checks if the API accepts the cookie
    /// </summary>
    /// <param name="cookie">Cookie string</param>
    /// <returns>False if the cookie is not valid</returns>
    public static bool ValidateCookie(string cookie)
    {
        if (string.IsNullOrWhiteSpace(cookie))
            return false;
        
        var requiredKeys = new[] { "__cf_bm", "cf_clearance", "PHPSESSID" };
    
        foreach (var key in requiredKeys)
        {
            if (!cookie.Contains(key + "="))
                return false;
        }
    
        return true;
    }


    #region Constructor

    /// <summary>
    /// Pixiv client allowing to interact with the Pixiv API
    /// </summary>
    /// <param name="cookie">Cookie to authenticate with the API</param>
    /// <param name="clientHandler">Custom HTTP client handler for testing or other cases</param>
    public PixivClient(string cookie, HttpClientHandler? clientHandler = null)
    {
        if (ValidateCookie(cookie) == false)
        {
            throw new PixivException("Invalid cookie. The cookie should be in the format of '__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;' in any order.");
        }

        _httpClient = new HttpClient(clientHandler ?? new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All });
        _httpClient.BaseAddress = new Uri(BaseUriHttps);

        _httpClient.DefaultRequestHeaders.Add("Priority", "u=1, i");
        _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);
        _httpClient.DefaultRequestHeaders.Add("Referer", BaseUriHttps);
    }

    #endregion



    #region Common Method


    private async Task<T> CommonGetAsync<T>(string url, JsonTypeInfo<PixivResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var wrapper = await _httpClient.GetFromJsonAsync(url, jsonTypeInfo, cancellationToken);
        if (wrapper?.Error ?? true)
        {
            throw new PixivException(wrapper?.Message);
        }
        return wrapper.Body;
    }


    private async Task<T> CommonPostAsync<T>(string url, object value, JsonTypeInfo<PixivResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(url, value, PixivJsonSerializerContext.Default.Object, cancellationToken);
        response.EnsureSuccessStatusCode();
        var wrapper = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
        if (wrapper?.Error ?? true)
        {
            throw new PixivException(wrapper?.Message);
        }
        return wrapper.Body;
    }


    private async Task CommonSendAsync<T>(HttpRequestMessage message, JsonTypeInfo<PixivResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();
        var wrapper = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
        if (wrapper?.Error ?? true)
        {
            throw new PixivException(wrapper?.Message);
        }
    }


    /// <summary>
    /// Retrieves the cross-site request forgery token and adds it to the request headers
    /// </summary>
    /// <returns>True if the token was successfully set</returns>
    public async Task<bool> GetTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(this._token))
        {
            return true;
        }
        var str = await _httpClient.GetStringAsync("/");
        _token = Regex.Match(str, @"""token"":""([^""]+)""").Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(_token))
        {
            _httpClient.DefaultRequestHeaders.Add("x-csrf-token", _token);
            return true;
        }
        else
        {
            return false;
        }
    }




    #endregion



    #region User

    /// <summary>
    /// Retrieves the current UID. Returns 0 if not logged
    /// </summary>
    /// <returns>The user UID or 0 if not logged</returns>
    public async Task<int> GetMyUserIdAsync()
    {
        var url = "/";
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        if (response.Headers.TryGetValues("x-userid", out var idstr))
        {
            var ids = idstr.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ids))
            {
                if (int.TryParse(ids, out var id))
                {
                    return id;
                }
            }
        }
        return 0;
    }


    /// <summary>
    /// Retrives information for the specified user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UserInfo> GetUserInfoAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}?full=1";
        return await CommonGetAsync<UserInfo>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserInfo, cancellationToken);
    }

    /// <summary>
    /// Get the top works for the specified user
    /// </summary>
    /// <returns></returns>
    public async Task<UserTopWorks> GetUserTopWorksAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/profile/top";
        return await CommonGetAsync<UserTopWorks>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserTopWorks, cancellationToken);
    }

    /// <summary>
    /// Get all works for the specified user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The all ids for illustrations, mangas, ...</returns>
    public async Task<UserAllWorks> GetUserAllWorksAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/profile/all";
        return await CommonGetAsync<UserAllWorks>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserAllWorks,cancellationToken);
    }

    #endregion



    #region Illust


    /// <summary>
    /// 插画详情
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IllustInfo> GetIllustInfoAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/{illustId}";
        return await CommonGetAsync<IllustInfo>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperIllustInfo, cancellationToken);
    }

    /// <summary> Get the information for the provided illustration ids</summary>
    /// <param name="userId"></param>
    /// <param name="illustIds"></param>
    /// <param name="useEnglish"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IllustWorks> GetUserIllustsAsync(int userId, ICollection<int> illustIds, bool useEnglish = false, CancellationToken cancellationToken = default)
    {
        if (illustIds.Count > 24)
        {
            throw new ArgumentException("The maximum amount of illustrations retrieved in a single call is 24", nameof(illustIds));
        }
        
        var queryString = HttpUtility.ParseQueryString(String.Empty);

        foreach (var illustId in illustIds)
        {
            queryString.Add("ids[]", illustId.ToString(NumberFormatInfo.InvariantInfo));
        }
        
        queryString.Add("work_category", "illust");
        queryString.Add("is_first_page", "1");

        if (useEnglish)
        {
            queryString.Add("lang", "en");
        }
        
        var url = $"/ajax/user/{userId}/profile/illusts?{queryString}";
        return await CommonGetAsync<IllustWorks>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperIllustWorks, cancellationToken);
    }

    /// <summary>
    /// Get the list of user illustrations filtered by tag
    /// </summary>
    /// <param name="userId">Id of the user</param>
    /// <param name="tag">Tag to filter by</param>
    /// <param name="page">Page to search. Starts at 1</param>
    /// <param name="limit">Number of items to retrieve. Recommended to keep at default unless the API changes</param>
    /// <param name="lang"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UserIllustsByTag> GetUserIllustsByTagAsync(int userId, string tag, uint page, uint limit = 48, SearchLanguage? lang = null, CancellationToken cancellationToken = default)
    {
        if (page == 0)
            page = 1;
        var queryString = HttpUtility.ParseQueryString(String.Empty);
        queryString["tag"] = tag;
        queryString["offset"] = ((page - 1) * limit).ToString(NumberFormatInfo.InvariantInfo);
        queryString["limit"] = limit.ToString(NumberFormatInfo.InvariantInfo);
        queryString["sensitiveFilterMode"] = "userSetting";
        if (lang is not null)
        {
            queryString["lang"] = lang.Value.ToStringFast(true);
        }

        var url = $"https://www.pixiv.net/ajax/user/{userId}/illusts/tag?{queryString}";
        var response = await CommonGetAsync<UserIllustsByTag>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserIllustsByTag, cancellationToken);
        return response;
    }

    /// <summary>
    /// 插画内所有图片（需登录）
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<IllustImage>> GetIllustPagesAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/{illustId}/pages";
        return await CommonGetAsync<List<IllustImage>>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperListIllustImage, cancellationToken);
    }

    /// <summary>
    /// Ugoira/Animation data (login required)
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<AnimateIllustMeta> GetAnimateIllustMetaAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/{illustId}/ugoira_meta";
        var data = await CommonGetAsync<AnimateIllustMeta>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperAnimateIllustMeta, cancellationToken);
        data.IllustId = illustId;
        return data;
    }


    /// <summary>
    /// 漫画系列
    /// </summary>
    /// <param name="seriesId">漫画系列 id</param>
    /// <param name="page">页数，倒序，一页12个</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<MangaSeries> GetMangaSeriesAsync(int seriesId, int page = 1, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/series/{seriesId}?p={page}";
        var response = await CommonGetAsync<MangaSeriesResponse>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperMangaSeriesResponse, cancellationToken);
        var manga = response.MangaSeries.First(x => x.Id == seriesId);
        var dicIlluts = response.Thumbnails.Illusts.ToDictionary(x => x.Id);
        var works = response.Page.Works;
        var illusts = new List<MangaSeriesIllust>(works.Count);
        foreach (var work in works)
        {
            if (dicIlluts.TryGetValue(work.WorkId, out var illust))
            {
                illusts.Add(new MangaSeriesIllust { IllustId = work.WorkId, IllustProfile = illust, Order = work.Order });
            }
        }
        manga.Illusts = illusts;
        return manga;
    }



    /// <summary>
    /// 追更漫画系列
    /// </summary>
    /// <param name="mangaSeriesId">小说系列id</param>
    /// <param name="unWatch">取消追更</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task WatchMangaSeriesAsync(int mangaSeriesId, bool unWatch = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/series/{mangaSeriesId}/{(unWatch ? "unwatch" : "watch")}";
        await CommonPostAsync<JsonNode>(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 更改漫画系列追更通知，追更漫画系列后再开启通知
    /// </summary>
    /// <param name="mangaSeriesId">小说系列id</param>
    /// <param name="enable">开启关闭通知</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ChangeMangaSeriesWatchListNotification(int mangaSeriesId, bool enable = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/series/{mangaSeriesId}/watchlist/notification/{(enable ? "turn_on" : "turn_off")}";
        await CommonPostAsync<JsonNode>(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 插画主页内容
    /// </summary>
    /// <returns></returns>
    private async Task GetIllustHomePageAsync(CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/top/illust?mode=all";
        // todo
    }


    /// <summary>
    /// 漫画主页内容
    /// </summary>
    /// <returns></returns>
    private async Task GetMangaHomePageAsync(CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/top/manga?mode=all";
        // todo
    }


    /// <summary>
    /// 给插画点赞，好像不能取消
    /// </summary>
    /// <param name="illustId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task LikeIllustAsync(int illustId, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/like";
        var obj = new { illust_id = illustId.ToString(NumberFormatInfo.InvariantInfo) };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 收藏插画，返回收藏id
    /// </summary>
    /// <param name="illustId">插画id</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="comment">收藏时附加的评论</param>
    /// <param name="tags">收藏时添加的标签，使用未翻译的原始标签，最多10个</param>
    /// <param name="cancellationToken"></param>
    /// <returns>收藏id</returns>
    public async Task<long> AddBookmarkIllustAsync(int illustId, bool isPrivate = false, string comment = "",  CancellationToken cancellationToken = default, params string[] tags)
    {
        var request = new AddBookmarkIllustRequest
        {
            IllustId = illustId,
            IsPrivate = isPrivate,
            Comment = comment,
            Tags = tags.Take(10),
        };
        return await AddBookmarkIllustAsync(request, cancellationToken);
    }


    /// <summary>
    /// 收藏插画，返回收藏id
    /// </summary>
    /// <param name="request">收藏请求</param>
    /// <param name="cancellationToken"></param>
    /// <returns>收藏id</returns>
    public async Task<long> AddBookmarkIllustAsync(AddBookmarkIllustRequest request, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/bookmarks/add";
        var jsonNode = await CommonPostAsync<JsonNode>(url, request, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
        var conversionResult = long.TryParse((string?)jsonNode["last_bookmark_id"], out var bookmarkId);
        if (!conversionResult)
        {
            throw new PixivException("Failed to parse bookmark id to return. \n" +
                                     "Received: \n" +
                                     jsonNode.ToJsonString() +
                                     "\n Expected to find numeric value in property 'last_bookmark_id'.");
        }
        return bookmarkId;
    }


    /// <summary>
    /// 删除收藏的插画
    /// </summary>
    /// <param name="bookmarkId">收藏id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteBookmarkIllustAsync(int bookmarkId, CancellationToken cancellationToken = default)
    {
        const string url = "/rpc/index.php";
        var form = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("mode", "delete_illust_bookmark"),
            new KeyValuePair<string, string>("bookmark_id", bookmarkId.ToString(NumberFormatInfo.InvariantInfo))
        };
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new FormUrlEncodedContent(form),
        };
        await CommonSendAsync<JsonNode>(message, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 批量更改收藏插画的公开属性
    /// </summary>
    /// <param name="isPrivate">不公开</param>
    /// <param name="bookmarkIds">收藏id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ChangeBookmarkIllustVisibilityAsync(bool isPrivate, CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/illusts/bookmarks/edit_restrict";
        var obj = new { bookmarkIds = bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)), bookmarkRestrict = isPrivate ? "private" : "public" };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }



    /// <summary>
    /// 批量给收藏插画添加自定义标签
    /// </summary>
    /// <param name="bookmarkIds">收藏id</param>
    /// <param name="tags">自定义标签，添加后每个插画的标签不超过10个</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task AddBookmarkIllustTagsAsync(IEnumerable<long> bookmarkIds, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/bookmarks/add_tags";
        var obj = new { bookmarkIds = bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)), tags };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 批量删除搜藏的插画
    /// </summary>
    /// <param name="bookmarkIds">收藏id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteBookmarkIllustsAsync(CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/illusts/bookmarks/remove";
        var obj = new { bookmarkIds = bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)) };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 相关推荐插画，推荐插画可能很多，将分批返回数据
    /// </summary>
    /// <param name="illustId">插画id</param>
    /// <param name="batchSize">每批返回多少个</param>
    /// <param name="cancellationToken"></param>
    /// <returns>可能为空</returns>
    public async IAsyncEnumerable<IEnumerable<IllustProfile>> GetRecommendIllustsAsync(int illustId, int batchSize = 20, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initUrl = $"/ajax/illust/{illustId}/recommend/init?limit={batchSize}";
        var response = await CommonGetAsync<RecommendIllustWrapper>(initUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendIllustWrapper, cancellationToken);
        // 竟然有广告
        yield return response.Illusts.Where(x => x.Id != 0);
        foreach (var ids in response.NextIds.Chunk(batchSize))
        {
            var nextUrl = $"/ajax/illust/recommend/illusts?{string.Join("&", ids.Select(x => $"illust_ids[]={x}"))}";
            yield return (await CommonGetAsync<RecommendIllustWrapper>(nextUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendIllustWrapper, cancellationToken)).Illusts.Where(x => x.Id != 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Stream> DownloadIllustAsync(string illustUrl, CancellationToken cancellationToken = default)
    {
        var response = await _downloadHttpClient.GetAsync(illustUrl, cancellationToken);
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        return content;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="illustUrl"></param>
    /// <param name="destinationStream"></param>
    /// <param name="cancellationToken"></param>
    public async Task DownloadIllustAsync(string illustUrl, Stream destinationStream, CancellationToken cancellationToken = default)
    {
        using var response = await _downloadHttpClient.GetAsync(illustUrl, cancellationToken);
        response.EnsureSuccessStatusCode();
        await response.Content.CopyToAsync(destinationStream, cancellationToken);
    }


    #endregion



    #region Novel


    /// <summary>
    /// 小说详情（包含正文）
    /// </summary>
    /// <param name="novelId">小说id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<NovelInfo> GetNovelInfoAsync(int novelId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/{novelId}";
        return await CommonGetAsync<NovelInfo>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperNovelInfo, cancellationToken);
    }

    /// <summary>
    /// 小说系列（无章节信息）
    /// </summary>
    /// <param name="novelSeriesId">小说id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<NovelSeries> GetNovelSeriesAsync(int novelSeriesId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series/{novelSeriesId}";
        return await CommonGetAsync<NovelSeries>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperNovelSeries, cancellationToken);
    }

    /// <summary>
    /// 小说系列的章节信息（无正文）
    /// </summary>
    /// <param name="novelSeriesId">小说id</param>
    /// <param name="offset">章节偏移量，按章节正序</param>
    /// <param name="limit">章节数限制</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<NovelSeriesChapter>> GetNovelSeriesChaptersAsync(int novelSeriesId, int offset, int limit = 30, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series_content/{novelSeriesId}?limit={limit}&last_order={offset}&order_by=asc";
        var wrapper = await CommonGetAsync<NovelSeriesContentWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperNovelSeriesContentWrapper, cancellationToken);
        return wrapper.Page.SeriesContents;
    }


    /// <summary>
    /// 追更小说系列
    /// </summary>
    /// <param name="novelSeriesId">小说系列id</param>
    /// <param name="unWatch">取消追更</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task WatchNovelSeriesAsync(int novelSeriesId, bool unWatch = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series/{novelSeriesId}/{(unWatch ? "unwatch" : "watch")}";
        await CommonPostAsync<JsonNode>(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 更改小说系列追更通知，追更小说系列后再开启通知
    /// </summary>
    /// <param name="novelSeriesId">小说系列id</param>
    /// <param name="enable">开启关闭通知</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ChangeNovelSeriesWatchListNotification(int novelSeriesId, bool enable = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series/{novelSeriesId}/watchlist/notification/{(enable ? "turn_on" : "turn_off")}";
        await CommonPostAsync<JsonNode>(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 小说主页内容
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task GetNovelHomePageAsync(CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/top/novel?mode=all";
        // todo
    }


    /// <summary>
    /// 给小说点赞，好像不能取消
    /// </summary>
    /// <param name="novelId">小说id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task LikeNovelAsync(int novelId, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/novels/like";
        var obj = new { novel_id = novelId.ToString(NumberFormatInfo.InvariantInfo) };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 给小说的某一页加上书签
    /// </summary>
    /// <param name="myUserId">我的uid</param>
    /// <param name="novelId">小说id</param>
    /// <param name="page">页数，大于 0 标记，等于 0 删除标记</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task MarkerNovelPageAsync(int myUserId, int novelId, int page, CancellationToken cancellationToken = default)
    {
        const string url = "/novel/rpc_marker.php";
        var form = new List<KeyValuePair<string, string>>
        {
            new("mode", "save"),
            new("i_id", novelId.ToString(NumberFormatInfo.InvariantInfo)),
            new("u_id", myUserId.ToString(NumberFormatInfo.InvariantInfo)),
            new("page", page.ToString(NumberFormatInfo.InvariantInfo)),
        };
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new FormUrlEncodedContent(form),
        };
        await _httpClient.SendAsync(message, cancellationToken);
    }


    /// <summary>
    /// 收藏小说，返回收藏id
    /// </summary>
    /// <param name="novelId">小说id</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="comment">收藏时附加的评论</param>
    /// <param name="tags">收藏时添加的标签，最多10个（小说标签没有翻译）</param>
    /// <param name="cancellationToken"></param>
    /// <returns>收藏id</returns>
    public async Task<long> AddBookmarkNovelAsync(int novelId, bool isPrivate = false, string comment = "", CancellationToken cancellationToken = default, params string[] tags)
    {
        var request = new AddBookmarkNovelRequest
        {
            NovelId = novelId,
            IsPrivate = isPrivate,
            Comment = comment,
            Tags = tags.Take(10),
        };
        return await AddBookmarkNovelAsync(request, cancellationToken);
    }


    /// <summary>
    /// 收藏小说，返回收藏id
    /// </summary>
    /// <param name="request">收藏请求</param>
    /// <param name="cancellationToken"></param>
    /// <returns>收藏id</returns>
    public async Task<long> AddBookmarkNovelAsync(AddBookmarkNovelRequest request, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/novels/bookmarks/add";
        var result = await CommonPostAsync<string>(url, request, PixivJsonSerializerContext.Default.PixivResponseWrapperString, cancellationToken);
        var conversionResult =  long.TryParse(result, out var bookmarkId);
        return bookmarkId;
    }


    /// <summary>
    /// 删除收藏的小说
    /// </summary>
    /// <param name="bookmarkId">小说id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteBookmarkNovelAsync(int bookmarkId, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/novels/bookmarks/delete";
        var form = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("del", "1"),
            new KeyValuePair<string, string>("book_id", bookmarkId.ToString(NumberFormatInfo.InvariantInfo))
        };
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new FormUrlEncodedContent(form),
        };
        await CommonSendAsync<JsonNode>(message, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 批量更改收藏小说的公开属性
    /// </summary>
    /// <param name="isPrivate">不公开</param>
    /// <param name="bookmarkIds">收藏id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ChangeBookmarkNovelVisibilityAsync(bool isPrivate, CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/novels/bookmarks/edit_restrict";
        var obj = new { bookmarkIds = bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)), bookmarkRestrict = isPrivate ? "private" : "public" };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 批量给收藏的小说添加自定义标签
    /// </summary>
    /// <param name="bookmarkIds">小说id</param>
    /// <param name="tags">自定义标签，添加后每个小说的标签不超过10个</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task AddBookmarkNovelTagsAsync(IEnumerable<long> bookmarkIds, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/novels/bookmarks/add_tags";
        var obj = new { bookmarkIds = bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)), tags = tags.Take(10) };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 删除收藏的小说
    /// </summary>
    /// <param name="bookmarkIds">小说id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteBookmarkNovelsAsync(CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/novels/bookmarks/remove";
        var obj = new { bookmarkIds = bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)) };
        await CommonPostAsync<JsonNode>(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// 相关推荐小说，推荐的小说可能很多，将分批返回数据
    /// </summary>
    /// <param name="novelId">小说id</param>
    /// <param name="batchSize">每批返回多少个</param>
    /// <param name="cancellationToken"></param>
    /// <returns>可能为空</returns>
    public async IAsyncEnumerable<IEnumerable<NovelProfile>> GetRecommendNovelsAsync(int novelId, int batchSize = 10, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initUrl = $"/ajax/novel/{novelId}/recommend/init?limit={batchSize}";
        var response = await CommonGetAsync<RecommendNovelWrapper>(initUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendNovelWrapper, cancellationToken);
        // 竟然有广告
        yield return response.Novels.Where(x => x.Id != 0);
        foreach (var ids in response.NextIds.Chunk(batchSize))
        {
            var nextUrl = $"/ajax/novel/recommend/novels?{string.Join("&", ids.Select(x => $"novelIds[]={x}"))}";
            yield return (await CommonGetAsync<RecommendNovelWrapper>(nextUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendNovelWrapper, cancellationToken)).Novels.Where(x => x.Id != 0);
        }
    }



    #endregion



    #region Bookmark


    /// <summary>
    /// 用户已收藏的插画数量
    /// </summary>
    /// <param name="userId">用户uid</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> GetUserBookmarkIllustCountAsync(int userId, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/illusts/bookmarks?tag=&offset=0&limit=1&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync<BookmarkIllustWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkIllustWrapper, cancellationToken);
        return wrapper.Total;
    }



    /// <summary>
    /// 收藏的插画
    /// </summary>
    /// <param name="userId">用户uid</param>
    /// <param name="offset">偏移量</param>
    /// <param name="limit">返回数量，返回数可能小于此数</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="tag">过滤标签</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<IllustProfile>> GetUserBookmarkIllustsAsync(int userId, int offset, int limit = 48, bool isPrivate = false, string? tag = null, CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, 100);
        if (!string.IsNullOrWhiteSpace(tag))
        {
            tag = UrlEncoder.Default.Encode(tag);
        }
        var url = $"/ajax/user/{userId}/illusts/bookmarks?tag={tag}&offset={offset}&limit={limit}&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync<BookmarkIllustWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkIllustWrapper, cancellationToken);
        return wrapper.Works;
    }


    /// <summary>
    /// 已收藏插画的所有自定义标签，包括公开和非公开，标签数过多则不会返回全部内容
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="returnEnglish"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UserBookmarkTag> GetUserBookmarkIllustTagsAsync(int userId, bool returnEnglish = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/illusts/bookmark/tags{(returnEnglish ? "?lang=en" : string.Empty)}";
        return await CommonGetAsync<UserBookmarkTag>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserBookmarkTag, cancellationToken);
    }


    /// <summary>
    /// 收藏的小说数量
    /// </summary>
    /// <param name="userId">用户uid</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> GetUserBookmarkNovelCountAsync(int userId, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/novels/bookmarks?tag=&offset=0&limit=1&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync<BookmarkNovelWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkNovelWrapper, cancellationToken);
        return wrapper.Total;
    }


    /// <summary>
    /// 收藏的小说
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="offset">偏移量</param>
    /// <param name="limit">返回数量，返回数可能小于此数</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="tag">过滤标签</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<NovelProfile>> GetUserBookmarkNovelsAsync(int userId, int offset, int limit = 24, string? tag = null, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, 100);
        if (!string.IsNullOrWhiteSpace(tag))
        {
            tag = Uri.EscapeDataString(tag);
        }
        var url = $"/ajax/user/{userId}/novels/bookmarks?tag={tag}&offset={offset}&limit={limit}&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync<BookmarkNovelWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkNovelWrapper, cancellationToken);
        return wrapper.Works;
    }


    /// <summary>
    /// 已收藏插画的所有自定义标签，包括公开和非公开，标签数过多则不会返回全部内容
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UserBookmarkTag> GetUserBookmarkNovelTagsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/novels/bookmark/tags";
        return await CommonGetAsync<UserBookmarkTag>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserBookmarkTag, cancellationToken);
    }


    #endregion



    #region Following


    /// <summary>
    /// 已关注用户的数量
    /// </summary>
    /// <param name="userId">用户uid</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> GetFollowingUserCountAsync(int userId, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/following?offset=0&limit=1&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync<FollowingUserWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingUserWrapper, cancellationToken);
        return wrapper.Total;
    }



    /// <summary>
    /// 已关注的用户
    /// </summary>
    /// <param name="userId">用户uid</param>
    /// <param name="offset">偏移量</param>
    /// <param name="limit">返回数据量</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<FollowingUser>> GetFollowingUsersAsync(int userId, int offset, int limit = 100, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 0, 100);
        var url = $"/ajax/user/{userId}/following?offset={offset}&limit={limit}&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync<FollowingUserWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingUserWrapper, cancellationToken);
        return wrapper.Users;
    }

    /// <summary>
    /// 已关注用户的最新插画漫画作品
    /// </summary>
    /// <param name="page">第几页，最多35页，35页后的内容和35页相同</param>
    /// <param name="onlyR18">仅显示r18作品</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<IllustProfile>> GetFollowingUserLatestIllustsAsync(int page, bool onlyR18 = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/follow_latest/illust?p={page}&mode={(onlyR18 ? "r18" : "all")}";
        var wrapper = await CommonGetAsync<FollowingLatestWorkWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingLatestWorkWrapper, cancellationToken);
        return wrapper.Thumbnails.Illusts;
    }

    /// <summary>
    /// 已关注用户的最新小说作品
    /// </summary>
    /// <param name="page">第几页，最多35页，35页后的内容和35页相同</param>
    /// <param name="onlyR18">仅显示r18作品</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<NovelProfile>> GetFollowingUserLatestNovelsAsync(int page, bool onlyR18 = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/follow_latest/novel?p={page}&mode={(onlyR18 ? "r18" : "all")}";
        var wrapper = await CommonGetAsync<FollowingLatestWorkWrapper>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingLatestWorkWrapper, cancellationToken);
        return wrapper.Thumbnails.Novels;
    }


    /// <summary>
    /// 添加关注用户
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task AddFollowingUserAsync(int userId, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        const string url = "/bookmark_add.php";
        var form = new List<KeyValuePair<string, string>>
        {
            new("mode", "add"),
            new("type", "user"),
            new("user_id", userId.ToString(NumberFormatInfo.InvariantInfo)),
            new("tag", ""),
            new("restrict", isPrivate ? "1" : "0"),
            new("format", "json"),
        };
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new FormUrlEncodedContent(form),
        };
        await _httpClient.SendAsync(message, cancellationToken);
    }


    /// <summary>
    /// 删除关注用户
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteFollowingUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        const string url = "/rpc_group_setting.php";
        var form = new List<KeyValuePair<string, string>>
        {
            new("mode", "del"),
            new("type", "bookuser"),
            new("id", userId.ToString(CultureInfo.InvariantCulture)),
        };
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new FormUrlEncodedContent(form),
        };
        await _httpClient.SendAsync(message, cancellationToken);
    }


    /// <summary>
    /// 更改已关注用户的公开属性
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="isPrivate">不公开</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ChangeFollowingUserVisibilityAsync(int userId, bool isPrivate, CancellationToken cancellationToken = default)
    {
        const string url = "/rpc/index.php";
        var form = new List<KeyValuePair<string, string>>
        {
            new("mode", "following_user_restrict_change"),
            new("user_id", userId.ToString(NumberFormatInfo.InvariantInfo)),
            new("restrict", isPrivate ? "1" : "0"),
        };
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new FormUrlEncodedContent(form),
        };
        await _httpClient.SendAsync(message, cancellationToken);
    }


    /// <summary>
    /// 新关注用户后推荐的相关用户
    /// </summary>
    /// <param name="userId">关注的用户id</param>
    /// <param name="userNumber">推荐的用户数量</param>
    /// <param name="workNumber">每个用户展示的作品数</param>
    /// <param name="allowR18">允许展示r18作品（存疑）</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<RecommendUser>> GetRecommendAfterFollowingUserAsync(int userId, int userNumber = 20, int workNumber = 3, bool allowR18 = true, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/recommends?userNum={userNumber}&workNum={workNumber}&isR18={allowR18}";
        var response = await CommonGetAsync<RecommendUserResponse>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendUserResponse, cancellationToken);
        var dicIllust = response.Thumbnails.Illusts.ToDictionary(x => x.Id);
        var dicNovel = response.Thumbnails.Novels.ToDictionary(x => x.Id);
        var dicMap = response.RecommendMaps.ToDictionary(x => x.UserId);
        foreach (var user in response.Users)
        {
            var illusts = new List<IllustProfile>(workNumber);
            var novels = new List<NovelProfile>(workNumber);
            if (dicMap.TryGetValue(user.UserId, out var map))
            {
                foreach (var illustIdStr in map.IllustIds)
                {
                    if (int.TryParse(illustIdStr, out var illustId))
                    {
                        if (dicIllust.TryGetValue(illustId, out var illust))
                        {
                            illusts.Add(illust);
                        }
                    }
                }
                foreach (var novelIdStr in map.NovelIds)
                {
                    if (int.TryParse(novelIdStr, out var novelId))
                    {
                        if (dicNovel.TryGetValue(novelId, out var novel))
                        {
                            novels.Add(novel);
                        }
                    }
                }
            }
            user.Illusts = illusts;
            user.Novels = novels;
        }
        return response.Users;
    }


    #endregion




    #region Search



    /// <summary>
    /// 搜索推荐
    /// </summary>
    /// <returns></returns>
    private async Task GetSearchSuggestionAsync(CancellationToken cancellationToken)
    {
        const string url = "/ajax/search/suggestion?mode=all";
        // todo
    }


    /// <summary>
    /// 修改喜欢的标签
    /// </summary>
    /// <param name="tags">所有标签</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ChangeFavoriteTags(IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/favorite_tags/save";
        await CommonPostAsync<JsonNode>(url, new { tags }, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }



    /// <summary>
    /// 搜索候选词
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<SearchCandidate>> GetSearchCandidatesAsync(string keyword, CancellationToken cancellationToken = default)
    {
        var url = $"/rpc/cps.php?keyword={keyword}";
        var node = await CommonGetAsync<JsonNode>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
        if (node["candidates"] is JsonArray array)
        {
            var list = array.Deserialize<List<SearchCandidate>>(PixivJsonSerializerContext.Default.ListSearchCandidate);
            bool? any = false;
            foreach (var candidate in list)
            {
                any = true;
                break;
            }

            if (any ?? false)
            {
                return list;
            }
        }
        return new List<SearchCandidate>();
    }


    

    /// <summary>
    /// Search all illustrations by keywords
    /// </summary>
    /// <param name="page">Page to retrieve</param>
    /// <param name="keywords">Search term to use</param>
    /// <param name="orderBy">Ordering of the search</param>
    /// <param name="searchAge">Safe, R18 or all</param>
    /// <param name="searchTarget">Kind of search</param>
    /// <param name="searchExact">If the tag of the work must exactly match with the provided keyword.
    /// If more than 1 keyword is passed, partial search will be used.</param>
    /// <param name="lang">Language of the search to correctly populate the tag translation property</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IllustSearchResult> SearchIllustrationsAsync(int page, string[] keywords, SearchOrder orderBy, 
        SearchAge searchAge = SearchAge.AnyAge, SearchTarget searchTarget = SearchTarget.IllustAndUgoira, 
        bool searchExact = true, SearchLanguage? lang = null, CancellationToken cancellationToken = default)
    {
        if (keywords.Length == 0)
        {
            throw new ArgumentException("Keywords cannot be empty", nameof(keywords));
        }
        var queryString = HttpUtility.ParseQueryString(String.Empty);
        var keyword = string.Join(" ", keywords);
        if (keywords.Length != 1 || 
            (keywords.Length == 1 && keywords[0].Contains(' '))
            )
        {
            searchExact = false;
        }

        queryString["word"] = keyword;
        queryString["order"] = orderBy.ToStringFast(true);
        queryString["mode"] = searchAge.ToStringFast(true);
        queryString["p"] = page.ToString(NumberFormatInfo.InvariantInfo);
        queryString["csw"] = 0.ToString(NumberFormatInfo.InvariantInfo);
        queryString["s_mode"] = searchExact ? "s_tag_full" : "s_tag";
        queryString["type"] = searchTarget.ToStringFast(true);
        if (lang is not null)
        {
            queryString["lang"] = lang.Value.ToStringFast(true);
        }
        
        var baseUrl = $"/ajax/search/illustrations/{keyword}";
        var url = $"{baseUrl}?{queryString}";

        var response = await CommonGetAsync<IllustSearchResult>(url, PixivJsonSerializerContext.Default.PixivResponseWrapperIllustSearchResult ,cancellationToken);
        return response; 
    }




    #endregion

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
    /// Disposes the internal HTTP clients used for network communication.
    /// </summary>
    public virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _httpClient.Dispose();
        _downloadHttpClient.Dispose();

    }
}