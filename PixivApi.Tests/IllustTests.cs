using System.Net;
using System.Text;
using System.Text.Json;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.Search;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.SerializerContexts;

namespace PixivApi.Tests;

[TestClass]
public sealed class IllustTests
{
    private TestHttpMessageHandler _handler;
    private PixivClient _pixivClient;

    [TestInitialize]
    public void Initialize()
    {
        _handler = new TestHttpMessageHandler();
        _pixivClient = new PixivClient(cfBm: "xxx", cfClearance: "yyy", phpsessid: "zzz", clientHandler: _handler);
    }

    private static HttpResponseMessage OkJson(string path) =>
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(File.ReadAllText(Path.Join("Payloads", path)), Encoding.UTF8, "application/json")
        };

    [TestMethod]
    [DataRow(56099861, "春の到来")]
    [DataRow(57826890, "夏、吹いてくる風")]
    public async Task GetIllustInfoAsync(int illustId, string title)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/illust/{illustId}",
            () => OkJson($"Illust/GetIllustInfo-{illustId}.json"));

        var illustInfo = await _pixivClient.GetIllustInfoAsync(illustId);

        Assert.AreEqual(title, illustInfo.Title);
    }

    [TestMethod]
    public async Task GetIllustHomePageAsync()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/top/illust?mode=all",
            () => OkJson("Illust/GetIllustHomePage.json"));
        
        var response = await _pixivClient.GetIllustHomePageAsync();
        Assert.HasCount(1614, response.TagTranslation); 
    }

    [TestMethod]
    [DataRow(68972163, 3)]
    public async Task GetIllustPagesAsync(int illustId, int expectedPages)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/illust/{illustId}/pages",
            () => OkJson("Illust/GetIllustPages.json"));

        var pages = await _pixivClient.GetIllustPagesAsync(illustId);

        Assert.HasCount(expectedPages, pages);
    }

    [TestMethod]
    [DataRow(49319675, 150)]
    public async Task GetAnimateIllustMetaAsync(int illustId, int frameCount)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/illust/{illustId}/ugoira_meta",
            () => OkJson("Illust/GetAnimateIllustMeta.json"));

        var illustInfo = await _pixivClient.GetAnimateIllustMetaAsync(illustId);

        Assert.HasCount(frameCount, illustInfo.Frames);
    }

    [TestMethod]
    [DataRow(236717, 18, 12, true, 1)]
    [DataRow(236717, 18, 6, false, 2)]
    public async Task GetMangaSeriesAsync(int seriesId, int totalChapters, int receivedChapters,
        bool hasNextPage, int page)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/series/{seriesId}?p={page}",
            () => OkJson($"Illust/GetMangaSeries{(page > 1 ? "LastPage" : "")}.json"));

        var mangaSeries = await _pixivClient.GetMangaSeriesAsync(seriesId, page);

        Assert.AreEqual(totalChapters, mangaSeries.Total);
        Assert.HasCount(receivedChapters, mangaSeries.Illusts);
        Assert.AreEqual(hasNextPage, mangaSeries.HasNextPage());
    }

    [DataRow(152148, 2536, "メイド")]
    [TestMethod]
    public async Task SearchAsync(int totalItems, int totalPages, params string[] keywords)
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/%E3%83%A1%E3%82%A4%E3%83%89?word=%E3%83%A1%E3%82%A4%E3%83%89&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en",
            () => OkJson("Illust/SearchIllustrations.json"));

        var result = await _pixivClient.SearchIllustrationsAsync(1, keywords, SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.AreEqual(totalPages, result.Illust.LastPage);
        Assert.AreEqual(totalItems, result.Illust.Total);
    }

    [TestMethod]
    [DataRow(233)]
    public async Task GetMangaHomePageAsync(int totalThumbnails)
    {
        _handler.When(
            "https://www.pixiv.net/ajax/top/manga?mode=all",
            () => OkJson("Illust/GetMangaHomePage.json"));
        
        var response = await _pixivClient.GetMangaHomePageAsync();
        Assert.IsNotNull(response.Thumbnails.Illusts);
        Assert.HasCount(totalThumbnails, response.Thumbnails.Illusts);
    }

    [TestMethod]
    [DataRow(207, 78, 53, 38)]
    public async Task GetSearchSuggestionAsync(int expectedThumbnailsCount, int expectedTagTranslationCount,
        int expectedIllustTagsCount, int expectedNovelTagsCount)
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/suggestion?mode=all",
            () => OkJson("Illust/GetSearchSuggestion.json"));
        
        var result = await _pixivClient.GetSearchSuggestionAsync();
        
        Assert.IsNotNull(result.Thumbnails);
        Assert.HasCount(expectedThumbnailsCount, result.Thumbnails);
        Assert.HasCount(expectedTagTranslationCount, result.TagTranslation);
        Assert.HasCount(expectedIllustTagsCount, result.PopularTags.Illust);
        Assert.HasCount(expectedNovelTagsCount, result.PopularTags.Novel!);
    }

    [TestMethod]
    [DataRow(24367326, 38)]
    public async Task GetUserIllustsAsync(int userId, int expectedItems)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}/profile/all",
            () => OkJson("User/GetUserAllWorks.json"));
        var userWorks = await _pixivClient.GetUserAllWorksAsync(userId);
        Assert.HasCount(expectedItems, userWorks.Illusts);
        
        _handler.When(
            $"https://www.pixiv.net/ajax/user/24367326/profile/illusts?ids%5b%5d=128990531&ids%5b%5d=125530786&ids%5b%5d=122076706&ids%5b%5d=119527627&ids%5b%5d=110983258&ids%5b%5d=110737731&ids%5b%5d=110557359&ids%5b%5d=110348477&ids%5b%5d=110146523&ids%5b%5d=110061240&ids%5b%5d=110007966&ids%5b%5d=109943143&ids%5b%5d=109907585&ids%5b%5d=109858269&ids%5b%5d=109804787&ids%5b%5d=109775465&ids%5b%5d=109710276&ids%5b%5d=109679794&ids%5b%5d=109651769&ids%5b%5d=109616169&ids%5b%5d=109586045&ids%5b%5d=109536119&ids%5b%5d=109510932&ids%5b%5d=109500041&ids%5b%5d=109483840&ids%5b%5d=109477822&ids%5b%5d=109453295&ids%5b%5d=105285735&ids%5b%5d=105086041&ids%5b%5d=104913623&ids%5b%5d=104884141&ids%5b%5d=104488303&ids%5b%5d=103713496&ids%5b%5d=103499942&ids%5b%5d=102698697&ids%5b%5d=101488016&ids%5b%5d=100970681&ids%5b%5d=100779246&work_category=illust&is_first_page=1",
            () => OkJson("Illust/GetUserIllusts.json"));
        var userIllusts = await _pixivClient.GetUserIllustsAsync(userId, userWorks.Illusts, ignoreItemLimit: true);
        Assert.IsGreaterThanOrEqualTo(expectedItems, userIllusts.Works.Count, $"Expected at least {expectedItems} illustrations, but got {userIllusts.Works.Count}");
    }

    [TestMethod]
    [DataRow(1152703, "名探偵プリキュア!")]
    public async Task GetUserIllustsByTagAsync(int userId, string tag)
    {
        var encodedTag = Uri.EscapeDataString(tag);
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}/illusts/tag?tag={encodedTag}&offset=0&limit=48&sensitiveFilterMode=userSetting&lang=en",
            () => OkJson("Illust/GetUserIllustsByTag.json"));
        
        var userIllusts = await _pixivClient.GetUserIllustsByTagAsync(userId, tag, 0, 48, SearchLanguage.English);
        Assert.HasCount(8, userIllusts.Works);
        Assert.IsTrue(userIllusts.Works.All(w => w.Tags.Contains(tag)), $"Not all illustrations contain the tag '{tag}'");
    }

    [TestMethod]
    public async Task WatchMangaSeriesAsync()
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illust/series/1/watch",
            () => OkJson("Illust/WatchMangaSeries.json"));
        
        await _pixivClient.WatchMangaSeriesAsync(1);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task ChangeMangaSeriesWatchListNotification(bool enableNotification)
    {
        _handler.When(
            HttpMethod.Post,
            $"https://www.pixiv.net/ajax/illust/series/1/watchlist/notification/turn_{(enableNotification ? "on" : "off")}",
            () => OkJson($"Illust/ChangeMangaSeriesWatchListNotification{(enableNotification ? "On" : "Off")}.json"));
        
        var response = await _pixivClient.ChangeMangaSeriesWatchListNotificationAsync(1, enableNotification);
        if (enableNotification)
        {
            Assert.IsNotNull(response.Notifiable);
        }
        else
        {
            Assert.IsNull(response.Notifiable);
        }
    }

    [TestMethod]
    public async Task LikeIllustAsync()
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illusts/like",
            () => OkJson("Illust/LikeIllust.json"));
        await _pixivClient.LikeIllustAsync(1);
    }

    [TestMethod]
     [DataRow(1, 36678156469)]
    public async Task AddBookmarkIllustAsync(int illustId, long bookmarkId)
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illusts/bookmarks/add",
            () => OkJson("Illust/AddBookmarkIllust.json"));

        var response = await _pixivClient.AddBookmarkIllustAsync(illustId);
        Assert.AreEqual(bookmarkId, response);
    }

    [TestMethod]
    [DataRow(1)]
    public async Task DeleteBookmarkIllustAsync(long bookmarkId)
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illusts/bookmarks/delete",
            () => OkJson("Illust/DeleteBookmarkIllust.json"));
        await _pixivClient.DeleteBookmarkIllustAsync(bookmarkId);
        
        // Verify serialization
        var data = new DeleteBookmarkIllustRequest(bookmarkId);
        var serializedData = JsonSerializer.Serialize(data, PixivJsonSerializerContext.Default.DeleteBookmarkIllustRequest);
        var expectedJsonString = $"{{\n  \"bookmark_id\": \"{bookmarkId}\"\n}}";
        Assert.AreEqual(expectedJsonString, serializedData);
    }
    
    [TestMethod]
    [DataRow(1, 2, 3)]
    public async Task DeleteBookmarkIllustAsync(params long[] bookmarkIds)
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illusts/bookmarks/delete",
            () => OkJson("Illust/DeleteBookmarkIllust.json"));

        var bookmarksToDelete = bookmarkIds.ToList();
        await _pixivClient.DeleteBookmarkIllustAsync(bookmarksToDelete);
        
        // Verify serialization
        var data = new DeleteBookmarkIllustBatchRequest(bookmarksToDelete);
        var serializedData = JsonSerializer.Serialize(data, PixivJsonSerializerContext.Default.DeleteBookmarkIllustBatchRequest);
        // Data structure is Dictionary<int, long[]>, where key is "bookmark_ids" and value is the array of bookmark IDs as strings
        var expectedJsonString = $$"""
                                  {
                                    "bookmarkIds": {
                                    "0": "{{bookmarkIds[0]}}",
                                    "1": "{{bookmarkIds[1]}}",
                                    "2": "{{bookmarkIds[2]}}"
                                  }
                                  }
                                  """;
        Assert.AreEqual(expectedJsonString, serializedData);
    }

    [TestMethod]
    [DataRow(1)]
    public async Task GetRecommendIllustsAsync(int illustId)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/illust/1/recommend/init?limit=20",
            () => OkJson("Illust/GetRecommendIllusts.json"));
        _handler.When(
            "https://www.pixiv.net/ajax/illust/recommend/illusts?illust_ids[]=143146290&illust_ids[]=143002778&illust_ids[]=144184649&illust_ids[]=142472837&illust_ids[]=142052537&illust_ids[]=143203732&illust_ids[]=143973515&illust_ids[]=142973225&illust_ids[]=141983551&illust_ids[]=144063878&illust_ids[]=143755204&illust_ids[]=143070477&illust_ids[]=143423270&illust_ids[]=143584008&illust_ids[]=143146671&illust_ids[]=143484883&illust_ids[]=144000377&illust_ids[]=144037009&illust_ids[]=143109195&illust_ids[]=143521513",
            () => OkJson("Illust/GetRecommendIllusts.json"));
        
        var illustsAsync = _pixivClient.GetRecommendIllustsAsync(1, 20);
        var acc = 0;
        var parsedIllustrations = new List<IllustProfile>();
        await foreach (var illusts in illustsAsync)
        {
            parsedIllustrations.AddRange(illusts);
            if(acc == 1)
                break;
            acc++;
        }
        Assert.IsNotNull(parsedIllustrations);
        Assert.HasCount(40, parsedIllustrations);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
