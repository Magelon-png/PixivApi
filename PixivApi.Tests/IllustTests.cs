using System.Web;
using Scighost.PixivApi;
using Scighost.PixivApi.Search;

namespace PixivApi.Tests;

[TestClass]
public sealed class IllustTests
{
    private readonly MockHttpClientHandler _handler = new MockHttpClientHandler();
    private PixivClient pixivClient;

    [TestInitialize]
    public void Initialize()
    {
        pixivClient = new PixivClient(cookie: "__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;", clientHandler: _handler);
    }

    [TestMethod]
    [DataRow(56099861, "春の到来")]
    [DataRow(57826890, "夏、吹いてくる風")]
    public async Task GetIllustInfoAsync(int illustId, string title)
    {
        var illustInfo = await pixivClient.GetIllustInfoAsync(illustId);
        
        Assert.AreEqual(title, illustInfo.Title);
    }

    [TestMethod]
    [DataRow(68972163, 3)]
    public async Task GetIllustPagesAsync(int illustId, int expectedPages)
    {
        var illustInfo = await pixivClient.GetIllustPagesAsync(illustId);
        
        Assert.AreEqual(expectedPages, illustInfo.Count);
    }

    [TestMethod]
    [DataRow(49319675, 150)]
    public async Task GetAnimateIllustMetaAsync(int illustId, int frameCount)
    {
        var illustInfo = await pixivClient.GetAnimateIllustMetaAsync(illustId);
        
        Assert.AreEqual(frameCount, illustInfo.Frames.Count);
    }

    [TestMethod]
    [DataRow(236717, 18, 12, true, 1)]
    [DataRow(236717, 18, 6, false, 2)]
    public async Task GetMangaSeriesAsync(int seriesId, int totalChapters, int receivedChapters, 
        bool hasNextPage, int page)
    {
        var mangaSeries = await pixivClient.GetMangaSeriesAsync(seriesId, page);
        
        Assert.AreEqual(totalChapters, mangaSeries.Total);
        
        Assert.AreEqual(receivedChapters, mangaSeries.Illusts.Count);
        
        Assert.AreEqual(hasNextPage, mangaSeries.HasNextPage());
    }

    [DataRow(
        152148,
        2536,
        "メイド")]
    [TestMethod]
    public async Task SearchAsync(int totalItems, int totalPages, params string[] keywords)
    {
        var result = await pixivClient.SearchIllustrationsAsync(1, keywords, SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchType.IllustAndUgoira, true, SearchLanguage.English);
        Assert.AreEqual(totalPages, result.Illust.LastPage);
        Assert.AreEqual(totalItems, result.Illust.Total);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        pixivClient.Dispose();
    }
}