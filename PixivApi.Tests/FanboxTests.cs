using System.Net;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.Fanbox;

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
        _fanboxClient = new FanboxClient(cfBm: "xxx", cfClearance: "yyy", fanboxsessid: "zzz", clientHandler: _handler);
    }

    private static HttpResponseMessage OkJson(string path) =>
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(File.ReadAllText(Path.Join("Payloads", path)), Encoding.UTF8, "application/json")
        };

    [TestMethod]
    public void ShouldFindCurlImpersonateExecutable()
    {
        using var client = new FanboxClient(cfBm: "xxx", cfClearance: "yyy", fanboxsessid: "zzz",
            enableCurlImpersonate: true);
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
            _handler.When(
                $"https://api.fanbox.cc/post.listSupporting?limit=10",
                () => OkJson("Fanbox/GetHomePagePosts.json"));
            _handler.When(
                $"https://api.fanbox.cc/post.listHome?limit=10",
                () => OkJson("Fanbox/GetHomePagePosts.json"));
            
            _handler.When("https://api.fanbox.cc/post.listHome?maxPublishedDatetime=2026-04-26%2004%3A19%3A57&maxId=11798168&limit=10", 
                () => OkJson("Fanbox/GetHomePagePosts-NextUrl.json"));
        
        var response = _fanboxClient.GetHomePagePostsAsync(supportedPostsOnly);
        Assert.IsNotNull(response);
        int iterations = 0;
        await foreach (var post in response)
        {
            iterations++;
        }
        Assert.AreEqual(10, iterations);
    }
    
    [TestMethod]
    public async Task GetNotificationAsync()
    {
        _handler.When(
            "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0",
            () => OkJson("Fanbox/GetNotification.json"));
        var nextUrl = "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0&commentOnly=0&lastId=notification123";
        _handler.When(nextUrl, () => OkJson("Fanbox/GetNotification-NextUrl.json"));

        var result = _fanboxClient.GetNotificationAsync();

        
        var iterations = 0;
        await foreach (var notification in result)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual("notification123", notification.Id);
            Assert.IsTrue(notification.IsUnread);
            iterations++;
        }
        
        Assert.AreEqual(1, iterations);
        
    }

    [TestMethod]
    public async Task GetNotificationAsync_WithCommentOnly()
    {
        _handler.When(
            "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0&commentOnly=1",
            () => OkJson("Fanbox/GetNotification.json"));
        var nextUrl = "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0&commentOnly=0&lastId=notification123";
        _handler.When(nextUrl, () => OkJson("Fanbox/GetNotification-NextUrl.json"));

        var result = _fanboxClient.GetNotificationAsync(commentOnly: true);

        var iterations = 0;
        
        await foreach (var notification in result)
        {
            Assert.IsNotNull(result);
            iterations++;
        }
        
        Assert.AreEqual(1, iterations);


    }

    [TestMethod]
    public async Task GetNotificationAsync_WithNextUrl()
    {
        _handler.When(
            "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0",
            () => OkJson("Fanbox/GetNotification.json"));
        var nextUrl = "https://api.fanbox.cc/bell.list?limit=10&skipConvertUnreadNotification=0&commentOnly=0&lastId=notification123";
        _handler.When(nextUrl, () => OkJson("Fanbox/GetNotification-NextUrl.json"));

        var result = _fanboxClient.GetNotificationAsync();

        var items = new List<NotificationContent>();
        await foreach (var notification in result)
        {
            items.Add(notification);
            Assert.IsNotNull(result);

        }
        Assert.HasCount(1, items);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _fanboxClient.Dispose();
    }
}
