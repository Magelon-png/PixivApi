using System.Net;
using System.Text;
using Scighost.PixivApi.Clients;

namespace PixivApi.Tests;

[TestClass]
public class FollowingTests
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
    public async Task GetFollowingUsersAsync()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/user/0/following?offset=0&limit=24&rest=show",
            () => OkJson("Following/GetFollowingUsers.json"));
        
        var users = await _pixivClient.GetFollowingUsersAsync(0, 0, 24);
        Assert.IsNotNull(users);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}