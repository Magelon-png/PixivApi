using System.Net;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Search;

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
        _pixivClient = new PixivClient(cookie: "__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;", clientHandler: _handler);
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
    [DataRow(68972163, 3)]
    public async Task GetIllustPagesAsync(int illustId, int expectedPages)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/illust/{illustId}/pages",
            () => OkJson("Illust/GetIllustPages.json"));

        var pages = await _pixivClient.GetIllustPagesAsync(illustId);

        Assert.AreEqual(expectedPages, pages.Count);
    }

    [TestMethod]
    [DataRow(49319675, 150)]
    public async Task GetAnimateIllustMetaAsync(int illustId, int frameCount)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/illust/{illustId}/ugoira_meta",
            () => OkJson("Illust/GetAnimateIllustMeta.json"));

        var illustInfo = await _pixivClient.GetAnimateIllustMetaAsync(illustId);

        Assert.AreEqual(frameCount, illustInfo.Frames.Count);
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
        Assert.AreEqual(receivedChapters, mangaSeries.Illusts.Count);
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

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
