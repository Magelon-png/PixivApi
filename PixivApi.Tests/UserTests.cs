using Scighost.PixivApi;

namespace PixivApi.Tests;

[TestClass]
public class UserTests
{
    private readonly MockHttpClientHandler _handler = new MockHttpClientHandler();
    private PixivClient pixivClient;

    [TestInitialize]
    public void Initialize()
    {
        pixivClient = new PixivClient(cookie: "__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;", clientHandler: _handler);
    }

    [TestMethod]
    public async Task GetMyUserIdAsync()
    {
        var userId = await pixivClient.GetMyUserIdAsync();
        Assert.AreEqual(1000, userId);
    }
    
    [TestMethod]
    [DataRow(24367326)]
    //[DataRow(30757805)]
    public async Task GetUserAllWorksAsync(int userId)
    {
        var userWorks = await pixivClient.GetUserAllWorksAsync(userId);

        Assert.AreEqual(38, userWorks.Illusts.Count);
            
        Assert.AreEqual(82, userWorks.Manga.Count);
        
        Assert.AreEqual(0, userWorks.Novels.Count);
        
        Assert.AreEqual(3, userWorks.MangaSeries.Count);
        Assert.IsTrue(userWorks.MangaSeries.Any(ms => ms is { Id: 281351, Total: 9 }));
        
        Assert.AreEqual(0, userWorks.NovelSeries.Count);
        
        Assert.AreEqual(3, userWorks.Pickup.Count);
        Assert.IsTrue(userWorks.Pickup.Any(
            p => p["title"]
                .GetValue<string>()
                .Equals("二人の神さま憑き", StringComparison.OrdinalIgnoreCase)));
    }
    
    [TestMethod]
    [DataRow(24367326)]
    public async Task GetUserInfoAsync(int userId)
    {
        var userInfo = await pixivClient.GetUserInfoAsync(userId);

        Assert.AreEqual("ふに・無9", userInfo.Name);
        Assert.IsTrue(userInfo.Partial);
        Assert.IsFalse(userInfo.IsBlocking);
    }

    [TestMethod]
    [DataRow(24367326)]
    public async Task GetUserTopWorksAsync(int userId)
    {
        var userTopWorks = await pixivClient.GetUserTopWorksAsync(userId);
        
        Assert.AreEqual(24, userTopWorks.Illusts.Count);
        Assert.IsTrue(userTopWorks.Illusts.Any(i => i.Id == 109500041 &&
                                                    i.Tags.Contains("HololiveID") &&
                                                    i.PageCount == 3));
        
        Assert.AreEqual(24, userTopWorks.Mangas.Count);
        
        Assert.AreEqual(0, userTopWorks.Novels.Count);
    }

    [TestCleanup]
    public void Cleanup()
    {
        pixivClient.Dispose();
    }
}