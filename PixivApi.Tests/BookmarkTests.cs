using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;

namespace PixivApi.Tests;

[TestClass]
public class BookmarkTests
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
    [DataRow("ホロライブ", "%E3%83%9B%E3%83%AD%E3%83%A9%E3%82%A4%E3%83%96")]
    public void UrlTagEncoding(string tag, string encodedTag)
    {
        Assert.AreEqual(encodedTag, UrlEncoder.Default.Encode(tag));
    }

    [TestMethod]
    [DataRow(1, true, 3155)]
    public async Task GetUserBookmarkIllustCountAsync(int userId, bool isPrivate, int expectedTotal)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}/illusts/bookmarks?tag=&offset=0&limit=1&rest=hide",
            () => OkJson("Bookmark/GetUserBookmarkIllusts.json"));

        var count = await _pixivClient.GetUserBookmarkIllustCountAsync(userId, isPrivate);

        Assert.AreEqual(expectedTotal, count);
    }

    [TestMethod]
    [DataRow(1, 192, 48, true, "アズールレーン")]
    public async Task GetUserBookmarkIllustsAsync(int userId, int offset, int limit, bool isPrivate, string tag)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}/illusts/bookmarks?tag=%E3%82%A2%E3%82%BA%E3%83%BC%E3%83%AB%E3%83%AC%E3%83%BC%E3%83%B3&offset={offset}&limit={limit}&rest=hide",
            () => OkJson("Bookmark/GetUserBookmarkIllusts.json"));

        var bookmarks = await _pixivClient.GetUserBookmarkIllustsAsync(userId, offset, limit, isPrivate, tag);

        Assert.AreEqual(limit, bookmarks.Count);
    }

    [TestMethod]
    [DataRow(1, true)]
    public async Task GetUserBookmarkIllustTagsAsync(int userId, bool returnEnglish)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}/illusts/bookmark/tags?lang=en",
            () => OkJson("Bookmark/GetUserBookmarkIllustTags.json"));

        var tags = await _pixivClient.GetUserBookmarkIllustTagsAsync(userId, returnEnglish);

        Assert.AreEqual(10, tags.Public[0].Count);
        Assert.AreEqual("未分類", tags.Public[0].Name);
        Assert.AreEqual(2, tags.Private.Sum(t => t.Count));
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task ChangeBookmarkIllustVisibilityAsync(bool isPrivate)
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illusts/bookmarks/edit_restrict",
            () => OkJson("Bookmark/ChangeBookmarkIllustVisibility.json"));

        await _pixivClient.ChangeBookmarkIllustVisibilityAsync(isPrivate, default, 123456789L, 987654321L);
    }

    [TestMethod]
    public async Task AddBookmarkIllustTagsAsync()
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illusts/bookmarks/add_tags",
            () => OkJson("Bookmark/ChangeBookmarkIllustVisibility.json"));

        await _pixivClient.AddBookmarkIllustTagsAsync(new long[] { 123456789L, 987654321L }, new[] { "tag1", "tag2" });
    }

    [TestMethod]
    public async Task DeleteBookmarkIllustsAsync()
    {
        _handler.When(
            HttpMethod.Post,
            "https://www.pixiv.net/ajax/illusts/bookmarks/remove",
            () => OkJson("Bookmark/ChangeBookmarkIllustVisibility.json"));

        await _pixivClient.DeleteBookmarkIllustsAsync(default, 123456789L, 987654321L);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
