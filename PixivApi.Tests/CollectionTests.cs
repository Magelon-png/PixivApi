using System.Net;
using System.Numerics;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.Search;

namespace PixivApi.Tests;

[TestClass]
public class CollectionTests
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
    public async Task GetRecommendedCollectionTagsAsync()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/collections/search/recommended_tags?lang=en",
            () => OkJson("Collections/GetRecommendedCollectionTags.json"));

        var result = await _pixivClient.GetRecommendedCollectionTagsAsync(SearchLanguage.English);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.RecommendedTags);
        Assert.IsTrue(result.RecommendedTags.Count > 0);
        Assert.IsNotNull(result.TagTranslation);
        Assert.IsTrue(result.TagTranslation.Count > 0);
    }

    [TestMethod]
    public async Task GetTopCollectionsAsync()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/top/collection?lang=en",
            () => OkJson("Collections/GetTopCollections.json"));

        var result = await _pixivClient.GetTopCollectionsAsync(SearchLanguage.English);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Thumbnails);
        Assert.IsNotNull(result.Thumbnails.Collection);
        Assert.IsTrue(result.Thumbnails.Collection.Count > 0);
        Assert.IsNotNull(result.Page);
        Assert.IsNotNull(result.ZoneConfig);
    }

    [TestMethod]
    public async Task SearchCollectionsAsync()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/collections/search?mode=safe&limit=20&offset=0&tags=test&lang=en",
            () => OkJson("Collections/SearchCollections.json"));

        var result = await _pixivClient.SearchCollectionsAsync(1, new[] { "test" }, SearchCollectionMode.Safe, 20, SearchLanguage.English);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Data);
        Assert.IsTrue(result.Data.Total > 0);
        Assert.IsNotNull(result.Data.Ids);
        Assert.IsTrue(result.Data.Ids.Count > 0);
    }

    [TestMethod]
    public async Task GetCollectionAsync()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/collection/35931581765300027379",
            () => OkJson("Collections/GetCollection.json"));

        var result = await _pixivClient.GetCollectionAsync(BigInteger.Parse("35931581765300027379"));

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Data);
        Assert.IsNotNull(result.Data.Detail);
        Assert.IsNotNull(result.Data.Detail.Tags);
    }

    [TestMethod]
    public async Task GetUserCollectionsAsync()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/user/84753896/profile/collections?ids=35931581765300027379",
            () => OkJson("Collections/GetUserCollections.json"));

        var result = await _pixivClient.GetUserCollectionsAsync(84753896, new[] { BigInteger.Parse("35931581765300027379") });

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Works);
        Assert.IsTrue(result.Works.Count > 0);
    }

    [TestMethod]
    public async Task GetBookmarkCollectionsAsync()
    {
        var userIdResponse = new HttpResponseMessage(HttpStatusCode.OK);
        userIdResponse.Headers.Add("x-userid", "12345");

        _handler.When(
            "https://www.pixiv.net/ajax/top/illust?mode=all",
            () => userIdResponse);

        _handler.When(
            "https://www.pixiv.net/ajax/user/12345/collections/bookmarks?offset=0&limit=24",
            () => OkJson("Collections/GetBookmarkCollection.json"));

        var result = await _pixivClient.GetBookmarkCollectionsAsync();

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Works);
        Assert.IsTrue(result.Works.Count > 0);
        Assert.IsTrue(result.Total > 0);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
