using System.Net;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;

namespace PixivApi.Tests;

[TestClass]
public class NovelTests
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
    [DataRow(23496777, "金木犀は苦手なの", 4366)]
    public async Task GetNovelInfoAsync(int novelId, string title, int characterCount)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/novel/{novelId}",
            () => OkJson("Novel/GetNovelInfo.json"));

        var novelInfo = await _pixivClient.GetNovelInfoAsync(novelId);

        Assert.IsTrue(novelInfo.Tags.Any(t => t.Tag == "autumntime2024"));
        Assert.AreEqual(title, novelInfo.Title);
        Assert.AreEqual(characterCount, novelInfo.CharacterCount);
    }

    [TestMethod]
    [DataRow(9256559, 0, 30)]
    public async Task GetNovelSeriesChaptersAsync(int novelSeriesId, int offset, int expectedEpisodes)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/novel/series_content/{novelSeriesId}?limit=30&last_order={offset}&order_by=asc",
            () => OkJson("Novel/GetNovelSeriesChapters.json"));

        var novelChapters = await _pixivClient.GetNovelSeriesChaptersAsync(novelSeriesId, offset);

        Assert.HasCount(expectedEpisodes, novelChapters);
    }

    [TestMethod]
    [DataRow(207)]
    public async Task GetNovelHomePageAsync(int expectedNovels)
    {
        _handler.When(
            "https://www.pixiv.net/ajax/top/novel?mode=all",
            () => OkJson("Novel/GetNovelHomePage.json"));
        
        var response = await _pixivClient.GetNovelHomePageAsync();
        Assert.IsNotNull(response.Thumbnails);
        Assert.HasCount(0, response.Thumbnails.Illusts);
        Assert.HasCount(expectedNovels, response.Thumbnails.Novels);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
