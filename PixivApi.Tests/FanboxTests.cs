using System.Net;
using System.Text;
using Scighost.PixivApi;

namespace PixivApi.Tests;

[TestClass]
public sealed class FanboxTests
{
    private TestHttpMessageHandler _handler;
    private FanboxClient _fanboxClient;

    [TestInitialize]
    public void Initialize()
    {
        _handler = new TestHttpMessageHandler();
        _fanboxClient = new FanboxClient(cookie: "__cf_bm=xxx;cf_clearance=yyy;FANBOXSESSID=zzz;", clientHandler: _handler);
    }

    private static HttpResponseMessage OkJson(string path) =>
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(File.ReadAllText(Path.Join("Payloads", path)), Encoding.UTF8, "application/json")
        };

    [TestMethod]
    public void ShouldFindCurlImpersonateExecutable()
    {
        using var client = new FanboxClient("__cf_bm=xxx;cf_clearance=yyy;FANBOXSESSID=zzz;",
            null, null, true);
    }

    [TestMethod]
    public async Task GetSupportingPlansAsync()
    {
        _handler.When(
            "https://api.fanbox.cc/plan.listSupporting",
            () => OkJson("Fanbox/GetSupportingPlans.json"));

        var plans = await _fanboxClient.GetSupportingPlansAsync();

        Assert.AreEqual(1, plans.Length);
    }

    [TestMethod]
    public async Task GetFollowedCreatorsAsync()
    {
        _handler.When(
            "https://api.fanbox.cc/creator.listFollowing",
            () => OkJson("Fanbox/GetFollowedCreators.json"));

        var creators = await _fanboxClient.GetFollowedCreatorsAsync();

        Assert.AreEqual(1, creators.Length);
    }

    [DataRow("emorimiku",
        "https://api.fanbox.cc/post.listCreator?creatorId=emorimiku&maxPublishedDatetime=2025-05-03%2015%3A01%3A05&maxId=9820948&limit=10",
        88)]
    [TestMethod]
    public async Task GetCreatorPostPaginationAsync(string creatorId, string urlToFind, int expectedPageCount)
    {
        _handler.When(
            $"https://api.fanbox.cc/post.paginateCreator?creatorId={creatorId}",
            () => OkJson("Fanbox/GetCreatorPostPagination.json"));

        var pages = await _fanboxClient.GetCreatorPostPaginationAsync(creatorId);

        Assert.AreEqual(expectedPageCount, pages.Length);
        Assert.IsTrue(pages.Any(p => p == urlToFind));
    }

    [DataRow(
        "https://api.fanbox.cc/post.listCreator?creatorId=emorimiku&maxPublishedDatetime=2025-05-03%2015%3A01%3A05&maxId=9820948&limit=10",
        10,
        9820948,
        true,
        20634476)]
    [TestMethod]
    public async Task GetCreatorPostsFromPaginationAsync(string url, int expectedPostCount, int postIdToFind,
        bool isRestricted, int expectedUserId)
    {
        _handler.When(url, () => OkJson("Fanbox/GetCreatorPostsFromPagination.json"));

        var postInfoList = await _fanboxClient.GetCreatorPostsFromPaginationAsync(url);

        Assert.AreEqual(expectedPostCount, postInfoList.Length);
        var expectedExistingPost = postInfoList.FirstOrDefault(p => p.Id == postIdToFind);
        Assert.IsNotNull(expectedExistingPost);
        Assert.AreEqual(postIdToFind, expectedExistingPost.Id);
        Assert.AreEqual(isRestricted, expectedExistingPost.IsRestricted);
        Assert.AreEqual(expectedUserId, expectedExistingPost.User.Id);
    }

    [DataRow(9345840, true, "emorimiku")]
    [TestMethod]
    public async Task GetPostInfoAsync(int postId, bool isRestricted, string creatorId)
    {
        _handler.When(
            $"https://api.fanbox.cc/post.info?postId={postId}",
            () => OkJson("Fanbox/GetPostInfo.json"));

        var postInfo = await _fanboxClient.GetPostInfoAsync(postId);

        Assert.AreEqual(postId, postInfo.Id);
        Assert.AreEqual(isRestricted, postInfo.IsRestricted);
        Assert.AreEqual(creatorId, postInfo.CreatorId);

        if (isRestricted)
        {
            Assert.IsNull(postInfo.Body);
        }
    }

    [TestCleanup]
    public void Cleanup()
    {
        _fanboxClient.Dispose();
    }
}
