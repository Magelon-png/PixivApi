using System.Net;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;

namespace PixivApi.Tests;

[TestClass]
public class UserTests
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
    public async Task GetMyUserIdAsync()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("x-userid", "1000");
        response.Content = new StringContent(File.ReadAllText(Path.Join("Payloads", "User/GetMyUserId.json")), Encoding.UTF8, "application/json");

        _handler.When("https://www.pixiv.net/ajax/top/illust?mode=all", () => response);

        var userId = await _pixivClient.GetMyUserIdAsync();

        Assert.AreEqual(1000, userId);
    }

    [TestMethod]
    [DataRow(24367326)]
    public async Task GetUserAllWorksAsync(int userId)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}/profile/all",
            () => OkJson("User/GetUserAllWorks.json"));

        var userWorks = await _pixivClient.GetUserAllWorksAsync(userId);

        Assert.HasCount(38, userWorks.Illusts);
        Assert.HasCount(82, userWorks.Manga);
        Assert.IsEmpty(userWorks.Novels);
        Assert.HasCount(3, userWorks.MangaSeries);
        Assert.IsTrue(userWorks.MangaSeries.Any(ms => ms is { Id: 281351, Total: 9 }));
        Assert.IsEmpty(userWorks.NovelSeries);
        Assert.HasCount(3, userWorks.Pickup);
        Assert.IsTrue(userWorks.Pickup.Any(
            p => p["title"]?
                .GetValue<string>()
                .Equals("二人の神さま憑き", StringComparison.OrdinalIgnoreCase) ?? false));
    }

    [TestMethod]
    [DataRow(24367326)]
    public async Task GetUserInfoAsync(int userId)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}?full=1",
            () => OkJson("User/GetUserInfo.json"));

        var userInfo = await _pixivClient.GetUserInfoAsync(userId);

        Assert.AreEqual("ふに・無9", userInfo.Name);
        Assert.IsTrue(userInfo.Partial);
        Assert.IsFalse(userInfo.IsBlocking);
    }

    [TestMethod]
    [DataRow(24367326)]
    public async Task GetUserTopWorksAsync(int userId)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/user/{userId}/profile/top",
            () => OkJson("User/GetUserTopWorks.json"));

        var userTopWorks = await _pixivClient.GetUserTopWorksAsync(userId);

        Assert.HasCount(24, userTopWorks.Illusts);
        Assert.IsTrue(userTopWorks.Illusts.Any(i => i.Id == 109500041 &&
                                                    i.Tags.Contains("HololiveID") &&
                                                    i.PageCount == 3));
        Assert.HasCount(24, userTopWorks.Mangas);
        Assert.IsEmpty(userTopWorks.Novels);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
