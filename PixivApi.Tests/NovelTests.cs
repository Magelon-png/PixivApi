using System.Text.Encodings.Web;
using Scighost.PixivApi;

namespace PixivApi.Tests;

[TestClass]
public class NovelTests
{
    private readonly MockHttpClientHandler _handler = new MockHttpClientHandler();
    private PixivClient pixivClient;

    [TestInitialize]
    public void Initialize()
    {
        pixivClient = new PixivClient(cookie: "__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;", clientHandler: _handler);
    }

    [TestMethod]
    [DataRow(23496777, "金木犀は苦手なの", 4366)]
    public async Task GetNovelInfoAsync(int novelId, string title, int characterCount)
    {
        var novelInfo = await pixivClient.GetNovelInfoAsync(novelId);

        Assert.IsTrue(novelInfo.Tags.Any(t => t.Tag == "autumntime2024"));

        Assert.AreEqual(title, novelInfo.Title);

        Assert.AreEqual(characterCount, novelInfo.CharacterCount);
    }

    // [TestMethod]
    // [DataRow(9256559, 620)]
    // public async Task GetNovelSeriesAsync(int novelSeriesId, int episodeCount)
    // {
    //     var novelInfo = await pixivClient.GetNovelSeriesAsync(novelSeriesId);
    //
    //     Assert.AreEqual(episodeCount, novelInfo.Total);
    // }

    [TestMethod]
    [DataRow(9256559, 0, 30)]
    public async Task GetNovelSeriesChaptersAsync(int novelSeriesId, int offset, int expectedEpisodes)
    {
        var novelChapters = await pixivClient.GetNovelSeriesChaptersAsync(novelSeriesId, offset);

        Assert.AreEqual(expectedEpisodes, novelChapters.Count);
    }

    

    [TestCleanup]
    public void Cleanup()
    {
        pixivClient.Dispose();
    }
}