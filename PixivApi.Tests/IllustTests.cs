using System.Net;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.Search;
using Scighost.PixivApi.Models.Illust;

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
    public async Task GetUserIllustsAsync()
    {
        //TODO
    }

    [TestMethod]
    public async Task GetUserIllustsByTagAsync()
    {
        //TODO
    }

    [TestMethod]
    public async Task WatchMangaSeriesAsync()
    {
        //TODO
    }

    [TestMethod]
    public async Task ChangeMangaSeriesWatchListNotification()
    {
        //TODO
    }

    [TestMethod]
    public async Task LikeIllustAsync()
    {
        //TODO
    }

    [TestMethod]
    public async Task AddBookmarkIllustAsync()
    {
        //TODO
    }

    [TestMethod]
    public async Task DeleteBookmarkIllustAsync()
    {
        //TODO
    }

    [TestMethod]
    public async Task GetRecommendIllustsAsync()
    {
        //TODO
    }

    [TestMethod]
    public async Task DownloadIllustAsync()
    {
        //TODO
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
