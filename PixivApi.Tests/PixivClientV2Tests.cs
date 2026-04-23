using System.Net;
using System.Reflection;
using System.Text;
using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.V2.Illust;
using Scighost.PixivApi.Models.V2.Novel;

namespace PixivApi.Tests;

[TestClass]
public sealed class PixivClientV2Tests
{
    private TestHttpMessageHandler _handler;
    private PixivClientV2 _pixivClientV2;

    [TestInitialize]
    public void Initialize()
    {
        _handler = new TestHttpMessageHandler();
        _pixivClientV2 = new PixivClientV2(null);

        // Set the HttpClient to use the test handler
        var httpClientField = typeof(PixivClientV2).GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);
        var newHttpClient = new HttpClient(_handler) { BaseAddress = new Uri("https://app-api.pixiv.net") };
        httpClientField?.SetValue(_pixivClientV2, newHttpClient);

        // Set fake token to avoid refresh
        var refreshTokenField = typeof(PixivClientV2).GetField("_refreshToken", BindingFlags.NonPublic | BindingFlags.Instance);
        refreshTokenField?.SetValue(_pixivClientV2, "fake_refresh_token");

        var tokenExpiresAtField = typeof(PixivClientV2).GetField("_tokenExpiresAt", BindingFlags.NonPublic | BindingFlags.Instance);
        tokenExpiresAtField?.SetValue(_pixivClientV2, DateTimeOffset.MaxValue);

        _pixivClientV2.HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer fake_token");
    }

    private static HttpResponseMessage OkJson(string path) =>
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(File.ReadAllText(Path.Join("Payloads", path)), Encoding.UTF8, "application/json")
        };

    // Tests for Illusts

    [TestMethod]
    public async Task GetUserIllustsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/user/illusts?user_id=123&offset=0&type=illust",
            () => OkJson("V2/Illusts/GetUserIllusts.json"));

        var result = await _pixivClientV2.GetUserIllustsAsync(123);

        Assert.IsNotNull(result);
        // Add assertions based on payload
    }

    [TestMethod]
    public async Task GetIllustDetailsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/detail?illust_id=456",
            () => OkJson("V2/Illusts/GetIllustDetails.json"));

        var result = await _pixivClientV2.GetIllustDetailsAsync(456);

        Assert.IsNotNull(result);
        // Add assertions
    }

    [TestMethod]
    public async Task GetUgoiraMetadataAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/ugoira/metadata?illust_id=789",
            () => OkJson("V2/Illusts/GetUgoiraMetadata.json"));

        var result = await _pixivClientV2.GetUgoiraMetadataAsync(789);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetIllustCommentsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/comments?illust_id=101",
            () => OkJson("V2/Illusts/GetIllustComments.json"));

        var result = await _pixivClientV2.GetIllustCommentsAsync(101);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetIllustsRanking()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/ranking?mode=day",
            () => OkJson("V2/Illusts/GetIllustsRanking.json"));

        var result = await _pixivClientV2.GetIllustsRanking(RankingMode.Day);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetRelatedIllusts()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/related?illust_id=202",
            () => OkJson("V2/Illusts/GetRelatedIllusts.json"));

        var result = await _pixivClientV2.GetRelatedIllusts(202);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetFollowIllustrationsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v2/illust/follow?restrict=public",
            () => OkJson("V2/Illusts/GetFollowIllustrations.json"));

        var result = await _pixivClientV2.GetFollowIllustrationsAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetIllustBookmarkDetailAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/bookmark/detail?illust_id=303",
            () => OkJson("V2/Illusts/GetIllustBookmarkDetail.json"));

        var result = await _pixivClientV2.GetIllustBookmarkDetailAsync(303);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetMyIllustrationsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v2/illust/mypixiv",
            () => OkJson("V2/Illusts/GetMyIllustrations.json"));

        var result = await _pixivClientV2.GetMyIllustrationsAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetNewIllustrationsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/new?content_type=illust",
            () => OkJson("V2/Illusts/GetNewIllustrations.json"));

        var result = await _pixivClientV2.GetNewIllustrationsAsync(IllustrationContentType.Illust);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetPopularIllustrationsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/popular?content_type=illust",
            () => OkJson("V2/Illusts/GetPopularIllustrations.json"));

        var result = await _pixivClientV2.GetPopularIllustrationsAsync(IllustrationContentType.Illust);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetRecommendedIllustrations()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/recommended?content_type=illust&include_ranking=False&include_ranking_label=False&offset=0",
            () => OkJson("V2/Illusts/GetRecommendedIllustrations.json"));

        var result = await _pixivClientV2.GetRecommendedIllustrations(IllustrationContentType.Illust);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetRecommendedIllustrationsNoLoginAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/illust/recommended-nologin?content_type=illust",
            () => OkJson("V2/Illusts/GetRecommendedIllustrationsNoLogin.json"));

        var result = await _pixivClientV2.GetRecommendedIllustrationsNoLoginAsync(IllustrationContentType.Illust);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchIllustsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/search/illust",
            () => OkJson("V2/Illusts/SearchIllusts.json"));

        var result = await _pixivClientV2.SearchIllustsAsync("test", SearchOrderV2.DateDescending);

        Assert.IsNotNull(result);
    }

    // Tests for Novels

    [TestMethod]
    public async Task GetFollowNovelsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/novel/follow?restrict=public",
            () => OkJson("V2/Novels/GetFollowNovels.json"));

        var result = await _pixivClientV2.GetFollowNovelsAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetRecommendedNovelsNoLoginAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/novel/recommended-nologin",
            () => OkJson("V2/Novels/GetRecommendedNovelsNoLogin.json"));

        var result = await _pixivClientV2.GetRecommendedNovelsNoLoginAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetMyNovelsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/novel/mypixiv",
            () => OkJson("V2/Novels/GetMyNovels.json"));

        var result = await _pixivClientV2.GetMyNovelsAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetNewNovelsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/novel/new",
            () => OkJson("V2/Novels/GetNewNovels.json"));

        var result = await _pixivClientV2.GetNewNovelsAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetNovelDetailAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v2/novel/detail?novel_id=404",
            () => OkJson("V2/Novels/GetNovelDetail.json"));

        var result = await _pixivClientV2.GetNovelDetailAsync(404);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetNovelCommentsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/novel/comments?novel_id=505",
            () => OkJson("V2/Novels/GetNovelComments.json"));

        var result = await _pixivClientV2.GetNovelCommentsAsync(505);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetNovelRankingAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/novel/ranking?mode=day",
            () => OkJson("V2/Novels/GetNovelRanking.json"));

        var result = await _pixivClientV2.GetNovelRankingAsync(RankingModeNovel.Day);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetNovelBookmarkDetailAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v2/novel/bookmark/detail?novel_id=606",
            () => OkJson("V2/Novels/GetNovelBookmarkDetail.json"));

        var result = await _pixivClientV2.GetNovelBookmarkDetailAsync(606);

        Assert.IsNotNull(result);
        Assert.AreEqual(BookmarkRestrictionScope.Public, result.BookmarkDetail.Restrict);
    }

    [TestMethod]
    public async Task GetNovelMarkersAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/novel/markers",
            () => OkJson("V2/Novels/GetNovelMarkers.json"));

        var result = await _pixivClientV2.GetNovelMarkersAsync();

        Assert.IsNotNull(result);
    }

    // Tests for Users

    [TestMethod]
    public async Task GetUserFollowDetailAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/user/follow/detail?user_id=707",
            () => OkJson("V2/Users/GetUserFollowDetail.json"));

        var result = await _pixivClientV2.GetUserFollowDetailAsync(707);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetUserBookmarkTagsIllustAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/user/bookmark-tags/illust?user_id=808&restrict=public",
            () => OkJson("V2/Users/GetUserBookmarkTagsIllust.json"));

        var result = await _pixivClientV2.GetUserBookmarkTagsIllustAsync(808);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetUserBookmarkTagsNovelAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/user/bookmark-tags/novel?user_id=909&restrict=public",
            () => OkJson("V2/Users/GetUserBookmarkTagsNovel.json"));

        var result = await _pixivClientV2.GetUserBookmarkTagsNovelAsync(909);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetUserBrowsingHistoryIllustsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/user/browsing-history/illusts",
            () => OkJson("V2/Users/GetUserBrowsingHistoryIllusts.json"));

        var result = await _pixivClientV2.GetUserBrowsingHistoryIllustsAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetUserBrowsingHistoryNovelsAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/user/browsing-history/novels",
            () => OkJson("V2/Users/GetUserBrowsingHistoryNovels.json"));

        var result = await _pixivClientV2.GetUserBrowsingHistoryNovelsAsync();

        Assert.IsNotNull(result);
    }

    // Tests for Miscellaneous

    [TestMethod]
    public async Task GetTrendingTagsIllustAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/trending-tags/illust",
            () => OkJson("V2/Misc/GetTrendingTagsIllust.json"));

        var result = await _pixivClientV2.GetTrendingTagsIllustAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetTrendingTagsNovelAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/trending-tags/novel",
            () => OkJson("V2/Misc/GetTrendingTagsNovel.json"));

        var result = await _pixivClientV2.GetTrendingTagsNovelAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetEmojisAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/emoji",
            () => OkJson("V2/Misc/GetEmojis.json"));

        var result = await _pixivClientV2.GetEmojisAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetApplicationInfoAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/application/info",
            () => OkJson("V2/Misc/GetApplicationInfo.json"));

        var result = await _pixivClientV2.GetApplicationInfoAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetWalkthroughListAsync()
    {
        _handler.When(
            "https://app-api.pixiv.net/v1/walkthrough/list",
            () => OkJson("V2/Misc/GetWalkthroughList.json"));

        var result = await _pixivClientV2.GetWalkthroughListAsync();

        Assert.IsNotNull(result);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClientV2.Dispose();
    }
}
