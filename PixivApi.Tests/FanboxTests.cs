using System.Web;
using Scighost.PixivApi;

namespace PixivApi.Tests;

[TestClass]
public sealed class FanboxTests
{
    private readonly MockHttpClientHandler _handler = new MockHttpClientHandler();
    private FanboxClient fanboxClient;

    [TestInitialize]
    public void Initialize()
    {
        fanboxClient = new FanboxClient(cookie: "__cf_bm=xxx;cf_clearance=yyy;FANBOXSESSID=zzz;", clientHandler: _handler);
    }

    [TestMethod]
    public void ShouldFindCurlImpersonateExecutable()
    {
        using var client = new FanboxClient("__cf_bm=xxx;cf_clearance=yyy;FANBOXSESSID=zzz;",
            null, null, true);
    }

    [TestMethod]
    public async Task GetSupportingPlansAsync()
    {
        var plans = await fanboxClient.GetSupportingPlansAsync();
        Assert.AreEqual(1, plans.Length);
    }
    
    [TestMethod]
    public async Task GetFollowedCreatorsAsync()
    {
        var creators= await fanboxClient.GetFollowedCreatorsAsync();
        Assert.AreEqual(1, creators.Length);
    }

    [DataRow("emorimiku", 
        "https://api.fanbox.cc/post.listCreator?creatorId=emorimiku&maxPublishedDatetime=2025-05-03%2015%3A01%3A05&maxId=9820948&limit=10",
        88)]
    [TestMethod]
    public async Task GetCreatorPostPaginationAsync(string creatorId, string urlToFind, int expectedPageCount)
    {
        var pages = await fanboxClient.GetCreatorPostPaginationAsync(creatorId);
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
        var postInfoList = await fanboxClient.GetCreatorPostsFromPaginationAsync(url);
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
        var postInfo = await fanboxClient.GetPostInfoAsync(postId);
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
        fanboxClient.Dispose();
    }
}