using System.Text.Encodings.Web;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;

namespace PixivApi.Tests;

[TestClass]
public class BookmarkTests
{
    private readonly MockHttpClientHandler _handler = new MockHttpClientHandler();
    private PixivClient pixivClient;

    [TestInitialize]
    public void Initialize()
    {
        pixivClient = new PixivClient(cookie: "__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;", clientHandler: _handler);
    }
    
    [TestMethod]
    [DataRow("ホロライブ", "%E3%83%9B%E3%83%AD%E3%83%A9%E3%82%A4%E3%83%96")]
    public void UrlTagEncoding(string tag, string encodedTag)
    {
        Assert.AreEqual(encodedTag, UrlEncoder.Default.Encode(tag));
    }

    [TestMethod]
    [DataRow(1, true, 3155)]
    public async Task GetUserBookmarkIllustCountAsync(int userId, bool isPrivate, int expectedTotal)
    {
        var count = await pixivClient.GetUserBookmarkIllustCountAsync(userId, isPrivate);
        
        Assert.AreEqual(expectedTotal, count);
    }
    
    [TestMethod]
    [DataRow(1, 192, 48, true, "アズールレーン")]
    public async Task GetUserBookmarkIllustsAsync(int userId, int offset, int limit, bool isPrivate, string tag)
    {
        var bookmarks = await pixivClient.GetUserBookmarkIllustsAsync(userId, offset, limit, isPrivate, tag);
        
        Assert.AreEqual(limit, bookmarks.Count);
    }
    
    [TestMethod]
    [DataRow(1, true)]

    public async Task GetUserBookmarkIllustTagsAsync(int userId, bool returnEnglish)
    {
        var tags = await pixivClient.GetUserBookmarkIllustTagsAsync(userId, returnEnglish);
        
        Assert.AreEqual(10, tags.Public[0].Count);
        
        Assert.AreEqual("未分類", tags.Public[0].Name);
        
        Assert.AreEqual(2, tags.Private.Sum(t => t.Count));
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        pixivClient.Dispose();
    }
}