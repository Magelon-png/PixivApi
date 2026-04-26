using System.Net;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;

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

        Assert.HasCount(1, plans);
    }
    
    [TestMethod]
    [DataRow("test", 2)]
    public async Task GetCreatorSupportingPlansAsync(string creatorId, int expectedPlanCount)
    {
        _handler.When(
            $"https://api.fanbox.cc/plan.listCreator?creatorId={creatorId}",
            () => OkJson("Fanbox/GetCreatorSupportingPlans.json"));

        var plans = await _fanboxClient.GetCreatorSupportingPlansAsync(creatorId);

        Assert.HasCount(2, plans);
    }

    [TestMethod]
    public async Task GetFollowedCreatorsAsync()
    {
        _handler.When(
            "https://api.fanbox.cc/creator.listFollowing",
            () => OkJson("Fanbox/GetFollowedCreators.json"));

        var creators = await _fanboxClient.GetFollowedCreatorsAsync();

        Assert.HasCount(1, creators);
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

        Assert.HasCount(expectedPageCount, pages);
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

        Assert.HasCount(expectedPostCount, postInfoList);
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

    [DataRow("test", 0, 109, 50, 1)]
    [TestMethod]
    public async Task GetCreatorPostsByCreatorIdAsync(string searchTerm, int currentPage, int expectedPostCount, int expectedItemCount,
        int? expectedNextPage)
    {
        _handler.When(
            $"https://api.fanbox.cc/creator.search?q={searchTerm}&page={currentPage}",
            () => OkJson("Fanbox/SearchCreators.json"));
        
        var result = await _fanboxClient.SearchCreatorsAsync(searchTerm, currentPage);
        
        Assert.AreEqual(expectedPostCount, result.Count);
        Assert.HasCount(expectedItemCount, result.Items);
        Assert.AreEqual(expectedNextPage, result.NextPage);
    }

    [TestMethod]
    public async Task GetRecommendedCreatorsAsync()
    {
        _handler.When(
            "https://api.fanbox.cc/creator.getRecommended",
            () => OkJson("Fanbox/GetRecommendedCreators.json"));

        var result = await _fanboxClient.GetRecommendedCreatorsAsync();

        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Creators);
        Assert.AreEqual("testcreator", result.Creators[0].CreatorId);
    }

    [TestMethod]
    public async Task GetRecommendedCreatorsAsync_WithLimit()
    {
        _handler.When(
            "https://api.fanbox.cc/creator.getRecommended?limit=10",
            () => OkJson("Fanbox/GetRecommendedCreators.json"));

        var result = await _fanboxClient.GetRecommendedCreatorsAsync(10);

        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Creators);
        Assert.AreEqual("testcreator", result.Creators[0].CreatorId);
    }

    [TestMethod]
    public async Task GetFollowedPixivCreators()
    {
        _handler.When(
            "https://api.fanbox.cc/creator.listPixiv",
            () => OkJson("Fanbox/GetRecommendedCreators.json"));

        var result = await _fanboxClient.GetFollowedPixivCreators();

        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Creators);
        Assert.AreEqual("testcreator", result.Creators[0].CreatorId);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task GetHomePagePostsAsync(bool supportedPostsOnly)
    {
        if(supportedPostsOnly)
        {
            _handler.When(
                $"https://api.fanbox.cc/post.listSupporting?limit=10",
                () => OkJson("Fanbox/GetHomePagePosts.json"));
        }
        else
        {
            _handler.When(
                $"https://api.fanbox.cc/post.listHome?limit=10",
                () => OkJson("Fanbox/GetHomePagePosts.json"));
        }
        
        var response = await _fanboxClient.GetHomePagePostsAsync(supportedPostsOnly);
        
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Items);
        Assert.HasCount(10, response.Items);
        
        _handler.When(response.NextUrl, 
            () => OkJson("Fanbox/GetHomePagePosts-NextUrl.json"));
        
        response = await _fanboxClient.GetHomePagePostsAsync(nextUrl: response.NextUrl);
        Assert.IsNotNull(response);
        Assert.IsNull(response.NextUrl);
        Assert.HasCount(0, response.Items);
    }
    
    [TestMethod]
    public async Task GetNotificationAsync()
    {
        _handler.When(
            "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0",
            () => OkJson("Fanbox/GetNotification.json"));

        var result = await _fanboxClient.GetNotificationAsync();

        Assert.IsNotNull(result);
        Assert.HasCount(1, result.items);
        Assert.AreEqual("notification123", result.items[0].Id);
        Assert.IsTrue(result.items[0].IsUnread);
    }

    [TestMethod]
    public async Task GetNotificationAsync_WithCommentOnly()
    {
        _handler.When(
            "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0&commentOnly=1",
            () => OkJson("Fanbox/GetNotification.json"));

        var result = await _fanboxClient.GetNotificationAsync(commentOnly: true);

        Assert.IsNotNull(result);
        Assert.HasCount(1, result.items);
    }

    [TestMethod]
    public async Task GetNotificationAsync_WithNextUrl()
    {
        var nextUrl = "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0&lastId=notification123";
        _handler.When(nextUrl, () => OkJson("Fanbox/GetNotification.json"));

        var result = await _fanboxClient.GetNotificationAsync(nextUrl: nextUrl);

        Assert.IsNotNull(result);
        Assert.HasCount(1, result.items);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _fanboxClient.Dispose();
    }
}
