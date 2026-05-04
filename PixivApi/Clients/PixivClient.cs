using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using System.Web;
using Polly;
using Scighost.PixivApi.Exceptions;
using Scighost.PixivApi.Helpers;
using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;
using Scighost.PixivApi.Models.Search;
using Scighost.PixivApi.Models.User;
using Scighost.PixivApi.SerializerContexts;

namespace Scighost.PixivApi.Clients;


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
    private const string DefaultUserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/146.0.0.0 Safari/537.36 Edg/146.0.0.0";


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
    
    private readonly ResiliencePipeline _resiliencePipeline;

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
    /// <param name="cfBm">__cf_bm cookie value</param>
    /// <param name="cfClearance">cf_clearance cookie value</param>
    /// <param name="phpsessid">PHPSESSID cookie value</param>
    /// <param name="additionalCookies">Additional cookies to append</param>
    /// <param name="clientHandler">Custom HTTP client handler for testing or other cases</param>
    /// <param name="userAgent">Custom user agent</param>
    public PixivClient(string cfBm, string cfClearance, string phpsessid, Dictionary<string, string>? additionalCookies = null, HttpMessageHandler? clientHandler = null, string? userAgent = null)
    {
        var cookie = $"__cf_bm={cfBm}; cf_clearance={cfClearance}; PHPSESSID={phpsessid};";
        if (additionalCookies != null)
        {
            foreach (var kvp in additionalCookies)
            {
                cookie += $" {kvp.Key}={kvp.Value};";
            }
        }

        if (ValidateCookie(cookie) == false)
        {
            throw new PixivException("Invalid cookie. The cookie should be in the format of '__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;' in any order.");
        }

        _resiliencePipeline = HttpClientHelper.GetResiliencePipeline();

        _httpClient = new HttpClient(clientHandler ?? new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All });
        _httpClient.BaseAddress = new Uri(BaseUriHttps);

        _httpClient.DefaultRequestVersion = HttpVersion.Version20;
        _httpClient.DefaultRequestHeaders.Add("Priority", "u=1, i");
        _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent ?? DefaultUserAgent);
        _httpClient.DefaultRequestHeaders.Add("Referer", BaseUriHttps);
        
        _downloadHttpClient.DefaultRequestVersion = HttpVersion.Version20;
    }

    #endregion



    #region Common Method


    private async Task<T> CommonGetAsync<T>(string url, JsonTypeInfo<PixivResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var wrapper = await _resiliencePipeline.ExecuteAsync(async token => await _httpClient.GetFromJsonAsync(url, jsonTypeInfo, token), cancellationToken
        ); 
        if (wrapper?.Error ?? true)
        {
            throw new PixivException(wrapper?.Message);
        }
        return wrapper.Body;
    }
    
    private async Task<T> CommonGetAsync<T>(string url, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var wrapper = await _resiliencePipeline.ExecuteAsync(async token => await _httpClient.GetFromJsonAsync(url, jsonTypeInfo, token), cancellationToken
        );
        if (wrapper is null)
        {
            throw new PixivException($"Could not retrieve data from {url} when it expected some.");
        }
        return wrapper!;
    }


    private async Task<T> CommonPostAsync<T>(string url, object value, JsonTypeInfo<PixivResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var response = await _resiliencePipeline.ExecuteAsync(
            async token =>
                await _httpClient.PostAsJsonAsync(url, value, PixivJsonSerializerContext.Default.Object, token),
            cancellationToken);
        var wrapper = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
        if (wrapper?.Error ?? true)
        {
            throw new PixivException(wrapper?.Message);
        }
        return wrapper.Body;
    }


    private async Task CommonSendAsync<T>(HttpRequestMessage message, JsonTypeInfo<PixivResponseWrapper<T>> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        var response =
            await _resiliencePipeline.ExecuteAsync(async token => await _httpClient.SendAsync(message, token),
                cancellationToken);
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
        
        var str = await _resiliencePipeline.ExecuteAsync(async token => await _httpClient.GetStringAsync("/", token));
        var match = Regex.Match(
            str,
            @"token\\?[""']\\?:\\?[""']([^""\\]+)\\?[""']"
        );

        _token = match.Success ? match.Groups[1].Value : string.Empty;
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
        var response = await _resiliencePipeline.ExecuteAsync(async token => await _httpClient.GetAsync("/ajax/top/illust?mode=all", token));
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
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="UserInfo"/> object containing the user's information</returns>
    public async Task<UserInfo> GetUserInfoAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}?full=1";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserInfo, cancellationToken);
    }

    /// <summary>
    /// Get the top works for the specified user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="UserTopWorks"/> object containing the user's top works</returns>
    public async Task<UserTopWorks> GetUserTopWorksAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/profile/top";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserTopWorks, cancellationToken);
    }

    /// <summary>
    /// Get all works for the specified user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="UserAllWorks"/> object containing the all ids for illustrations, mangas, etc.</returns>
    public async Task<UserAllWorks> GetUserAllWorksAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/profile/all";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserAllWorks,cancellationToken);
    }

    #endregion



    #region Illust


    /// <summary>
    /// Illustration details
    /// </summary>
    /// <param name="illustId">The ID of the illustration</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="IllustInfo"/> object containing the illustration details</returns>
    public async Task<IllustInfo> GetIllustInfoAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/{illustId}";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperIllustInfo, cancellationToken);
    }

    /// <summary> Get the information for the provided illustration ids. Should first call GetUserAllWorksAsync to get the list of IDs</summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="illustIds">The collection of illustration IDs. Should limit this to 24 items per call.</param>
    /// <param name="useEnglish">Whether to use English for the tags and other metadata</param>
    /// <param name="ignoreItemLimit">When true, this method will accept any number of ids. Although it is still recommended to limit this to 50 per call.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="IllustWorks"/> object containing the illustration information</returns>
    /// <exception cref="ArgumentException">Throws when more than 24 items are returned and ignoreItemLimit is false</exception>
    public async Task<IllustWorks> GetUserIllustsAsync(int userId, ICollection<int> illustIds, bool useEnglish = false, bool ignoreItemLimit = false, CancellationToken cancellationToken = default)
    {
        if (illustIds.Count > 24 && !ignoreItemLimit)
        {
            throw new ArgumentException("The maximum amount of illustrations retrieved in a single call is 24. Set ignoreItemLimit to true to ignore this limit.", nameof(illustIds));
        }
        
        var queryString = HttpUtility.ParseQueryString(String.Empty);

        foreach (var illustId in illustIds)
        {
            // In .NET 8, brackets are not being encoded at all
            #if NET8_0
            queryString.Add("ids%5b%5d", illustId.ToString(NumberFormatInfo.InvariantInfo));
            #else
            queryString.Add("ids[]", illustId.ToString(NumberFormatInfo.InvariantInfo));
            #endif
        }
        
        queryString.Add("work_category", "illust");
        queryString.Add("is_first_page", "1");

        if (useEnglish)
        {
            queryString.Add("lang", "en");
        }
        
        var url = $"/ajax/user/{userId}/profile/illusts?{queryString}";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperIllustWorks, cancellationToken);
    }

    /// <summary>
    /// Get the list of user illustrations filtered by tag
    /// </summary>
    /// <param name="userId">Id of the user</param>
    /// <param name="tag">Tag to filter by</param>
    /// <param name="page">Page to search. Starts at 1</param>
    /// <param name="limit">Number of items to retrieve. Recommended to keep at default unless the API changes</param>
    /// <param name="lang">Search language</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="UserIllustsByTag"/> object containing the filtered illustrations</returns>
    public async Task<UserIllustsByTag> GetUserIllustsByTagAsync(int userId, string tag, int page, int limit = 48, SearchLanguage? lang = null, CancellationToken cancellationToken = default)
    {
        if (page == 0)
            page = 1;
        var queryString = HttpUtility.ParseQueryString(String.Empty);
        queryString["offset"] = ((page - 1) * limit).ToString(NumberFormatInfo.InvariantInfo);
        queryString["limit"] = limit.ToString(NumberFormatInfo.InvariantInfo);
        queryString["sensitiveFilterMode"] = "userSetting";
        if (lang is not null)
        {
            queryString["lang"] = lang.Value.ToStringFast(true);
        }

        tag = Uri.EscapeDataString(tag);
        var url = $"https://www.pixiv.net/ajax/user/{userId}/illusts/tag?tag={tag}&{queryString}";
        var response = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserIllustsByTag, cancellationToken);
        return response;
    }

    /// <summary>
    /// All images in the illustration (requires login)
    /// </summary>
    /// <param name="illustId">The ID of the illustration</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="IllustImage"/> objects representing the pages of the illustration</returns>
    public async Task<List<IllustImage>> GetIllustPagesAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/{illustId}/pages";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperListIllustImage, cancellationToken);
    }

    /// <summary>
    /// Ugoira/Animation data (login required)
    /// </summary>
    /// <param name="illustId">The ID of the illustration</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="AnimateIllustMeta"/> object containing the animation data</returns>
    public async Task<AnimateIllustMeta> GetAnimateIllustMetaAsync(int illustId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/{illustId}/ugoira_meta";
        var data = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperAnimateIllustMeta, cancellationToken);
        data = data with
        {
            IllustId = illustId
        };
        return data;
    }


    /// <summary>
    /// Manga series
    /// </summary>
    /// <param name="seriesId">Manga series id</param>
    /// <param name="page">Page number, reverse order, 12 items per page</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="MangaSeries"/> object containing the manga series information</returns>
    public async Task<MangaSeries> GetMangaSeriesAsync(int seriesId, int page = 1, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/series/{seriesId}?p={page}";
        var response = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperMangaSeriesResponse, cancellationToken);
        var manga = response.MangaSeries.First(x => x.Id == seriesId);
        var dicIlluts = response.Thumbnails.Illusts.ToDictionary(x => x.Id);
        var works = response.Page.Works;
        var illusts = new List<MangaSeriesIllust>(works.Count);
        foreach (var work in works)
        {
            if (dicIlluts.TryGetValue(work.WorkId, out var illust))
            {
                illusts.Add(new MangaSeriesIllust(  IllustId: work.WorkId, IllustProfile: illust, Order: work.Order ));
            }
        }

        manga = manga with { Illusts = illusts };
        return manga;
    }



    /// <summary>
    /// Follow a manga series
    /// </summary>
    /// <param name="mangaSeriesId">Novel series id</param>
    /// <param name="unWatch">Unfollow</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task WatchMangaSeriesAsync(int mangaSeriesId, bool unWatch = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/series/{mangaSeriesId}/{(unWatch ? "unwatch" : "watch")}";
        await CommonPostAsync(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Change manga series follow notification settings, enable notifications after following the manga series
    /// </summary>
    /// <param name="mangaSeriesId">Novel series id</param>
    /// <param name="enable">Enable or disable notifications</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<ChangeMangaSeriesWatchListNotificationResponse> ChangeMangaSeriesWatchListNotificationAsync(int mangaSeriesId, bool enable = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/illust/series/{mangaSeriesId}/watchlist/notification/{(enable ? "turn_on" : "turn_off")}";
        return await CommonPostAsync(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperChangeMangaSeriesWatchListNotificationResponse, cancellationToken);
    }


    /// <summary>
    /// Illustration homepage content
    /// </summary>
    /// <returns></returns>
    public async Task<IllustHomePageResponse> GetIllustHomePageAsync(CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/top/illust?mode=all";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperIllustHomePageResponse, cancellationToken);
    }


    /// <summary>
    /// Manga homepage content
    /// </summary>
    /// <returns></returns>
    public async Task<MangaHomePageResponse> GetMangaHomePageAsync(CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/top/manga?mode=all";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperMangaHomePageResponse, cancellationToken);
    }


    /// <summary>
    /// Like an illustration (cannot be undone)
    /// </summary>
    /// <param name="illustId">The ID of the illustration</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task LikeIllustAsync(int illustId, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/like";
        var obj = new LikeIllustRequest(illustId);
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Bookmark an illustration, returns bookmark id
    /// </summary>
    /// <param name="illustId">Illustration id</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="comment">Additional comment when bookmarking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="tags">Tags added when bookmarking, use untranslated original tags, maximum 10</param>
    /// <returns>Bookmark id</returns>
    public async Task<long> AddBookmarkIllustAsync(int illustId, bool isPrivate = false, string comment = "",  CancellationToken cancellationToken = default, params string[] tags)
    {
        var request = new AddBookmarkIllustRequest
        (
            IllustId: illustId,
            IsPrivate: isPrivate,
            Comment: comment,
            Tags: tags.Take(10)
        );
        return await AddBookmarkIllustAsync(request, cancellationToken);
    }


    /// <summary>
    /// Bookmark an illustration, returns bookmark id
    /// </summary>
    /// <param name="request">Bookmark request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Bookmark id</returns>
    private async Task<long> AddBookmarkIllustAsync(AddBookmarkIllustRequest request, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/bookmarks/add";
        var response = await CommonPostAsync(url, request, PixivJsonSerializerContext.Default.PixivResponseWrapperAddBookmarkIllustResponse, cancellationToken);
        return response.BookmarkId;
    }

    /// <summary>
    /// Delete a bookmarked illustration
    /// </summary>
    /// <param name="bookmarkId">Bookmark id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task DeleteBookmarkIllustAsync(long bookmarkId, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/bookmarks/delete";
        var data = new DeleteBookmarkIllustRequest(bookmarkId);
        await CommonPostAsync(url, data, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }
    
    /// <summary>
    /// Delete a bookmarked illustration
    /// </summary>
    /// <param name="bookmarkIds">Bookmark ids</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task DeleteBookmarkIllustAsync(List<long> bookmarkIds, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/bookmarks/delete";
        var data = new DeleteBookmarkIllustBatchRequest(bookmarkIds);
        await CommonPostAsync(url, data, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// This function requires GetTokenAsync to have been called once.
    /// </summary>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="bookmarkIds">Bookmark ids</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task ChangeBookmarkIllustVisibilityAsync(bool isPrivate, CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/illusts/bookmarks/edit_restrict";
        var obj = new ChangeBookmarkIllustVisibilityRequest(
            bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)),
            isPrivate ? "private" : "public"
        );
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }



    /// <summary>
    /// Batch add custom tags to bookmarked illustrations
    /// </summary>
    /// <param name="bookmarkIds">Bookmark ids</param>
    /// <param name="tags">Custom tags, maximum 10 tags per illustration after adding</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task AddBookmarkIllustTagsAsync(IEnumerable<long> bookmarkIds, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/illusts/bookmarks/add_tags";
        var obj = new AddBookmarkIllustTagsRequest( 
            bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)),
             tags);
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Batch delete bookmarked illustrations
    /// </summary>
    /// <param name="bookmarkIds">Bookmark ids</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task DeleteBookmarkIllustsAsync(CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/illusts/bookmarks/remove";
        var obj = new DeleteBookmarkIllustsRequest(bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)));
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Related recommended illustrations. Recommended illustrations can be many, so data will be returned in batches.
    /// Called by pixiv when bookmarking an illustration 
    /// </summary>
    /// <param name="illustId">Illustration id</param>
    /// <param name="batchSize">Number of items per batch</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An asynchronous enumerable of illustration batches</returns>
    public async IAsyncEnumerable<IEnumerable<IllustProfile>> GetRecommendIllustsAsync(int illustId, int batchSize = 20, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initUrl = $"/ajax/illust/{illustId}/recommend/init?limit={batchSize}";
        var response = await CommonGetAsync(initUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendIllustWrapper, cancellationToken);
        // There are actually ads here
        yield return response.Illusts.Where(x => x.Id != 0);
        foreach (var ids in response.NextIds.Chunk(batchSize))
        {
            var nextUrl = $"/ajax/illust/recommend/illusts?{string.Join("&", ids.Select(x => $"illust_ids[]={x}"))}";
            yield return (await CommonGetAsync(nextUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendIllustWrapper, cancellationToken)).Illusts.Where(x => x.Id != 0);
        }
    }

    /// <summary>
    /// Downloads an illustration as a stream.
    /// </summary>
    /// <param name="illustUrl">The URL of the illustration to download</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="Stream"/> containing the illustration data</returns>
    public async Task<Stream> DownloadIllustAsync(string illustUrl, CancellationToken cancellationToken = default)
    {
        return await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var response = await _downloadHttpClient.GetAsync(illustUrl, token);
            var content = await response.Content.ReadAsStreamAsync(token);
        
            return content;
        }, cancellationToken);
        
    }
    
    /// <summary>
    /// Downloads an illustration and copies it to the provided destination stream.
    /// </summary>
    /// <param name="illustUrl">The URL of the illustration to download</param>
    /// <param name="destinationStream">The stream to which the illustration data will be copied</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation. Returns true if download is successful, false otherwise</returns>
    public async Task<bool> DownloadIllustAsync(string illustUrl, Stream destinationStream, CancellationToken cancellationToken = default)
    {
        var retries = 0;
        var maxRetries = 3;
        var downloaded = false;
        while (retries < maxRetries && !downloaded)
        {
            try
            {
                await _resiliencePipeline.ExecuteAsync(async token =>
                {
                    using var response = await _downloadHttpClient.GetAsync(illustUrl,
                        HttpCompletionOption.ResponseHeadersRead, token);
                    response.EnsureSuccessStatusCode();
                    await response.Content.CopyToAsync(destinationStream, token);
                    downloaded = true;
                }, cancellationToken);
            }
            catch
            {
                if (destinationStream.CanSeek)
                {
                    destinationStream.Seek(0, SeekOrigin.Begin);
                }

                await destinationStream.FlushAsync(cancellationToken);
                retries++;
            }
        }
        return downloaded;
    }
    
    /// <summary>
    /// Downloads an illustration and copies it to the provided destination stream.
    /// </summary>
    /// <param name="illustUrl">The URL of the illustration to download</param>
    /// <param name="destinationStream">The stream to which the illustration data will be copied</param>
    /// <param name="progress">Interface that can be used to follow the download progress</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation. Returns true if download is successful, false otherwise</returns>
    public async Task<bool> DownloadIllustAsync(string illustUrl, Stream destinationStream, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default)
    {
        var retries = 0;
        var maxRetries = 3;
        var downloaded = false;

        while (retries < maxRetries && !downloaded)
        {
            try
            {
                await _resiliencePipeline.ExecuteAsync(async token =>
                {
                    using var response = await _downloadHttpClient.GetAsync(
                        illustUrl,
                        HttpCompletionOption.ResponseHeadersRead,
                        token);

                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength;

                    await using var stream = await response.Content.ReadAsStreamAsync(token);

                    var buffer = new byte[81920];
                    long totalRead = 0;
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, token)) > 0)
                    {
                        await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead), token);

                        totalRead += bytesRead;

                        progress?.Report(new DownloadProgress(totalRead, totalBytes));
                    }

                    downloaded = true;
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                if (destinationStream.CanSeek)
                {
                    destinationStream.Seek(0, SeekOrigin.Begin);
                }

                await destinationStream.FlushAsync(cancellationToken);
                retries++;
            }
        }
        return downloaded;
    }

    #endregion



    #region Novel


    /// <summary>
    /// Novel details (including content)
    /// </summary>
    /// <param name="novelId">Novel id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="NovelInfo"/> object containing the novel details</returns>
    public async Task<NovelInfo> GetNovelInfoAsync(int novelId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/{novelId}";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperNovelInfo, cancellationToken);
    }

    /// <summary>
    /// Novel series (no chapter information)
    /// </summary>
    /// <param name="novelSeriesId">Novel series id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="NovelSeries"/> object containing the novel series information</returns>
    public async Task<NovelSeries> GetNovelSeriesAsync(int novelSeriesId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series/{novelSeriesId}";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperNovelSeries, cancellationToken);
    }

    /// <summary>
    /// Chapter information of a novel series (no content)
    /// </summary>
    /// <param name="novelSeriesId">Novel series id</param>
    /// <param name="offset">Chapter offset, in positive chapter order</param>
    /// <param name="limit">Chapter count limit</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="NovelSeriesChapter"/> objects</returns>
    public async Task<List<NovelSeriesChapter>> GetNovelSeriesChaptersAsync(int novelSeriesId, int offset, int limit = 30, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series_content/{novelSeriesId}?limit={limit}&last_order={offset}&order_by=asc";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperNovelSeriesContentWrapper, cancellationToken);
        return wrapper.Page.SeriesContents;
    }


    /// <summary>
    /// Follow a novel series
    /// </summary>
    /// <param name="novelSeriesId">Novel series id</param>
    /// <param name="unWatch">Unfollow</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task WatchNovelSeriesAsync(int novelSeriesId, bool unWatch = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series/{novelSeriesId}/{(unWatch ? "unwatch" : "watch")}";
        await CommonPostAsync(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Change novel series follow notification settings, enable notifications after following the novel series
    /// </summary>
    /// <param name="novelSeriesId">Novel series id</param>
    /// <param name="enable">Enable or disable notifications</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task ChangeNovelSeriesWatchListNotification(int novelSeriesId, bool enable = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/novel/series/{novelSeriesId}/watchlist/notification/{(enable ? "turn_on" : "turn_off")}";
        await CommonPostAsync(url, new object(), PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Novel homepage content
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public async Task<NovelHomePageResponse> GetNovelHomePageAsync(CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/top/novel?mode=all";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperNovelHomePageResponse, cancellationToken);
    }


    /// <summary>
    /// Like a novel (cannot be undone)
    /// </summary>
    /// <param name="novelId">Novel id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task LikeNovelAsync(int novelId, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/novels/like";
        var obj = new LikeNovelRequest(novelId);
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Add a bookmark to a specific page of a novel
    /// </summary>
    /// <param name="myUserId">My uid</param>
    /// <param name="novelId">Novel id</param>
    /// <param name="page">Page number, marker if > 0, delete marker if = 0</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
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
    /// Bookmark a novel, returns bookmark id
    /// </summary>
    /// <param name="novelId">Novel id</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="comment">Additional comment when bookmarking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="tags">Tags added when bookmarking, maximum 10 (novel tags have no translations)</param>
    /// <returns>Bookmark id</returns>
    public async Task<long> AddBookmarkNovelAsync(int novelId, bool isPrivate = false, string comment = "", CancellationToken cancellationToken = default, params string[] tags)
    {
        var request = new AddBookmarkNovelRequest
        (
            NovelId: novelId,
            IsPrivate: isPrivate,
            Comment: comment,
            Tags: tags.Take(10)
        );
        return await AddBookmarkNovelAsync(request, cancellationToken);
    }


    /// <summary>
    /// Bookmark a novel, returns bookmark id
    /// </summary>
    /// <param name="request">Bookmark request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Bookmark id</returns>
    public async Task<long> AddBookmarkNovelAsync(AddBookmarkNovelRequest request, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/novels/bookmarks/add";
        var result = await CommonPostAsync(url, request, PixivJsonSerializerContext.Default.PixivResponseWrapperString, cancellationToken);
        _ = long.TryParse(result, out var bookmarkId);
        return bookmarkId;
    }


    /// <summary>
    /// Delete a bookmarked novel
    /// </summary>
    /// <param name="bookmarkId">Novel id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
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
        await CommonSendAsync(message, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Batch change public visibility of bookmarked novels
    /// </summary>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="bookmarkIds">Bookmark ids</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task ChangeBookmarkNovelVisibilityAsync(bool isPrivate, CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/novels/bookmarks/edit_restrict";
        var obj = new ChangeBookmarkNovelVisibilityRequest(bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)), isPrivate ? "private" : "public");
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Batch add custom tags to bookmarked novels
    /// </summary>
    /// <param name="bookmarkIds">Novel id</param>
    /// <param name="tags">Custom tags, maximum 10 tags per novel after adding</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task AddBookmarkNovelTagsAsync(IEnumerable<long> bookmarkIds, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/novels/bookmarks/add_tags";
        var obj = new AddBookmarkNovelTagsRequest(bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)), tags.Take(10));
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Delete bookmarked novels
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="bookmarkIds">Novel id</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task DeleteBookmarkNovelsAsync(CancellationToken cancellationToken = default, params long[] bookmarkIds)
    {
        const string url = "/ajax/novels/bookmarks/remove";
        var obj = new DeleteBookmarkNovelsRequest(bookmarkIds.Select(x => x.ToString(NumberFormatInfo.InvariantInfo)) );
        await CommonPostAsync(url, obj, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }


    /// <summary>
    /// Related recommended novels. Recommended novels can be many, so data will be returned in batches.
    /// </summary>
    /// <param name="novelId">Novel id</param>
    /// <param name="batchSize">Number of items per batch</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An asynchronous enumerable of novel batches</returns>
    public async IAsyncEnumerable<IEnumerable<NovelProfile>> GetRecommendNovelsAsync(int novelId, int batchSize = 10, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initUrl = $"/ajax/novel/{novelId}/recommend/init?limit={batchSize}";
        var response = await CommonGetAsync(initUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendNovelWrapper, cancellationToken);
        // There are actually ads here
        yield return response.Novels.Where(x => x.Id != 0);
        foreach (var ids in response.NextIds.Chunk(batchSize))
        {
            var nextUrl = $"/ajax/novel/recommend/novels?{string.Join("&", ids.Select(x => $"novelIds[]={x}"))}";
            yield return (await CommonGetAsync(nextUrl, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendNovelWrapper, cancellationToken)).Novels.Where(x => x.Id != 0);
        }
    }



    #endregion



    #region Bookmark


    /// <summary>
    /// Number of illustrations bookmarked by the user
    /// </summary>
    /// <param name="userId">User uid</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The total number of bookmarked illustrations</returns>
    public async Task<int> GetUserBookmarkIllustCountAsync(int userId, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/illusts/bookmarks?tag=&offset=0&limit=1&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkIllustWrapper, cancellationToken);
        return wrapper.Total;
    }



    /// <summary>
    /// Bookmarked illustrations
    /// </summary>
    /// <param name="userId">User uid</param>
    /// <param name="offset">Offset</param>
    /// <param name="limit">Number of items to return, may be less than this number</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="tag">Filter tag</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="IllustProfile"/> objects</returns>
    public async Task<List<IllustProfile>> GetUserBookmarkIllustsAsync(int userId, int offset, int limit = 48, bool isPrivate = false, string? tag = null, CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, 100);
        if (!string.IsNullOrWhiteSpace(tag))
        {
            tag = UrlEncoder.Default.Encode(tag);
        }
        var url = $"/ajax/user/{userId}/illusts/bookmarks?tag={tag}&offset={offset}&limit={limit}&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkIllustWrapper, cancellationToken);
        return wrapper.Works;
    }


    /// <summary>
    /// All custom tags of bookmarked illustrations, including public and private. If there are too many tags, they may not all be returned.
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="returnEnglish">Whether to return English tags</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="UserBookmarkTag"/> object containing the tags</returns>
    public async Task<UserBookmarkTag> GetUserBookmarkIllustTagsAsync(int userId, bool returnEnglish = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/illusts/bookmark/tags{(returnEnglish ? "?lang=en" : string.Empty)}";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserBookmarkTag, cancellationToken);
    }


    /// <summary>
    /// Number of bookmarked novels
    /// </summary>
    /// <param name="userId">User uid</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The total number of bookmarked novels</returns>
    public async Task<int> GetUserBookmarkNovelCountAsync(int userId, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/novels/bookmarks?tag=&offset=0&limit=1&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkNovelWrapperV2, cancellationToken);
        return wrapper.Total;
    }


    /// <summary>
    /// Bookmarked novels
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="offset">Offset</param>
    /// <param name="limit">Number of items to return, may be less than this number</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="tag">Filter tag</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="NovelProfile"/> objects</returns>
    public async Task<List<NovelProfileBookmarks>> GetUserBookmarkNovelsAsync(int userId, int offset, int limit = 24, string? tag = null, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, 100);
        if (!string.IsNullOrWhiteSpace(tag))
        {
            tag = Uri.EscapeDataString(tag);
        }
        var url = $"/ajax/user/{userId}/novels/bookmarks?tag={tag}&offset={offset}&limit={limit}&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperBookmarkNovelWrapperV2, cancellationToken);
        return wrapper.Works;
    }


    /// <summary>
    /// All custom tags of bookmarked novels, including public and private. If there are too many tags, they may not all be returned.
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="UserBookmarkTag"/> object containing the tags</returns>
    public async Task<UserBookmarkTag> GetUserBookmarkNovelTagsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/novels/bookmark/tags";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperUserBookmarkTag, cancellationToken);
    }


    #endregion



    #region Following


    /// <summary>
    /// Number of followed users
    /// </summary>
    /// <param name="userId">User uid</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The total number of followed users</returns>
    public async Task<int> GetFollowingUserCountAsync(int userId, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/following?offset=0&limit=1&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingUserWrapper, cancellationToken);
        return wrapper.Total;
    }



    /// <summary>
    /// Followed users
    /// </summary>
    /// <param name="userId">User uid</param>
    /// <param name="offset">Offset</param>
    /// <param name="limit">Number of items to return</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="FollowingUser"/> objects</returns>
    public async Task<List<FollowingUser>> GetFollowingUsersAsync(int userId, int offset, int limit = 100, bool isPrivate = false, CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 0, 100);
        var url = $"/ajax/user/{userId}/following?offset={offset}&limit={limit}&rest={(isPrivate ? "hide" : "show")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingUserWrapper, cancellationToken);
        return wrapper.Users;
    }

    /// <summary>
    /// Latest illustration/manga works from followed users
    /// </summary>
    /// <param name="page">Page number, max 35. Content after page 35 is same as page 35</param>
    /// <param name="onlyR18">Only show R18 works</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="IllustProfile"/> objects</returns>
    public async Task<List<IllustProfile>> GetFollowingUserLatestIllustsAsync(int page, bool onlyR18 = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/follow_latest/illust?p={page}&mode={(onlyR18 ? "r18" : "all")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingLatestWorkWrapper, cancellationToken);
        return wrapper.Thumbnails.Illusts;
    }

    /// <summary>
    /// Latest novel works from followed users
    /// </summary>
    /// <param name="page">Page number, max 35. Content after page 35 is same as page 35</param>
    /// <param name="onlyR18">Only show R18 works</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="NovelProfile"/> objects</returns>
    public async Task<List<NovelProfile>> GetFollowingUserLatestNovelsAsync(int page, bool onlyR18 = false, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/follow_latest/novel?p={page}&mode={(onlyR18 ? "r18" : "all")}";
        var wrapper = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperFollowingLatestWorkWrapper, cancellationToken);
        return wrapper.Thumbnails.Novels;
    }


    /// <summary>
    /// Add follow user
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
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
    /// Delete follow user
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
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
    /// Change visibility of followed user
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="isPrivate">Private</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
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
    /// Recommended related users after following a new user
    /// </summary>
    /// <param name="userId">Followed user id</param>
    /// <param name="userNumber">Number of recommended users</param>
    /// <param name="workNumber">Number of works to show for each user</param>
    /// <param name="allowR18">Allow showing R18 works (uncertain)</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="RecommendUser"/> objects</returns>
    public async Task<List<RecommendUser>> GetRecommendAfterFollowingUserAsync(int userId, int userNumber = 20, int workNumber = 3, bool allowR18 = true, CancellationToken cancellationToken = default)
    {
        var url = $"/ajax/user/{userId}/recommends?userNum={userNumber}&workNum={workNumber}&isR18={allowR18}";
        var response = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperRecommendUserResponse, cancellationToken);
        var dicIllust = response.Thumbnails.Illusts.ToDictionary(x => x.Id);
        var dicNovel = response.Thumbnails.Novels.ToDictionary(x => x.Id);
        var dicMap = response.RecommendMaps.ToDictionary(x => x.UserId);
        for (var i = 0; i < response.Users.Count; i++)
        {
            var user = response.Users[i];
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
            response.Users[i] = user with
            {
                Illusts = illusts,
                Novels = novels
            };
        }
        return response.Users;
    }


    #endregion




    #region Search



    /// <summary>
    /// Search recommendations
    /// </summary>
    /// <returns></returns>
    public async Task<GetSearchSuggestionResponse> GetSearchSuggestionAsync(CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/search/suggestion?mode=all";
        return await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperGetSearchSuggestionResponse, cancellationToken);
    }


    /// <summary>
    /// Change favorite tags
    /// </summary>
    /// <param name="tags">All tags</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task ChangeFavoriteTags(IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        const string url = "/ajax/favorite_tags/save";
        await CommonPostAsync(url, new { tags }, PixivJsonSerializerContext.Default.PixivResponseWrapperJsonNode, cancellationToken);
    }



    /// <summary>
    /// Search candidates
    /// </summary>
    /// <param name="keyword">The keyword to search for</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of <see cref="SearchCandidate"/> objects</returns>
    public async Task<List<SearchCandidate>> GetSearchCandidatesAsync(string keyword, CancellationToken cancellationToken = default)
    {
        var url = $"/rpc/cps.php?keyword={keyword}";
        var response = await CommonGetAsync(url, PixivJsonSerializerContext.Default.SearchCandidateWrapper, cancellationToken);

        var list = response.Candidates;
        bool? any = false;
        foreach (var _ in list ?? [])
        {
            any = true;
            break;
        }

        if (any ?? false)
        {
            return list ?? [];
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
    /// <param name="hideAi">When true, search for non-AI generated illustrations</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A <see cref="IllustSearchResult"/> object containing the search results</returns>
    public async Task<IllustSearchResult> SearchIllustrationsAsync(int page, string[] keywords, SearchOrder orderBy,
        SearchAge searchAge = SearchAge.AnyAge, SearchTarget searchTarget = SearchTarget.IllustAndUgoira,
        bool searchExact = true, SearchLanguage? lang = null, bool hideAi = false, CancellationToken cancellationToken = default)
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
        if (hideAi)
        {
            queryString["ai_type"] = "1";
        }
        if (lang is not null)
        {
            queryString["lang"] = lang.Value.ToStringFast(true);
        }

        var baseUrl = $"/ajax/search/illustrations/{keyword}";
        var url = $"{baseUrl}?{queryString}";

        var response = await CommonGetAsync(url, PixivJsonSerializerContext.Default.PixivResponseWrapperIllustSearchResult ,cancellationToken);
        return response;
    }




    #endregion

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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

/// <summary>
/// Download progress information
/// </summary>
/// <param name="BytesRead">Current bytes read from the http call</param>
/// <param name="TotalBytes">Total bytes expected to be read from the http call</param>
public record DownloadProgress(long BytesRead, long? TotalBytes)
{
    /// <summary>
    /// Percentage of the download completed. Returns 100 if TotalBytes is null.
    /// </summary>
    public double? Percentage =>
        TotalBytes.HasValue ? (double)BytesRead / TotalBytes.Value * 100 : null;
}