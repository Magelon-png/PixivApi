using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scighost.PixivApi;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PixivApi.Tests;

[TestClass]
public class UserAgentTests
{
    private const string Cookie = "__cf_bm=xxx;cf_clearance=yyy;PHPSESSID=zzz;";
    private const string CustomUA = "CustomUA/1.0";

    [TestMethod]
    public async Task PixivClient_UsesCustomUserAgent()
    {
        var handler = new TestHttpMessageHandler();
        handler.When("https://www.pixiv.net/ajax/user/123?full=1", () => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"error\":false,\"body\":{}}")
        });

        var client = new PixivClient(Cookie, handler, CustomUA);
        await client.GetUserInfoAsync(123);

        Assert.AreEqual(CustomUA, handler.LastRequest?.Headers.UserAgent.ToString());
    }

    [TestMethod]
    public void PixivClient_UsesDefaultUserAgent_WhenNoneProvided()
    {
        var client = new PixivClient(Cookie);
        Assert.IsNotNull(client.HttpClient.DefaultRequestHeaders.UserAgent.ToString());
    }

    [TestMethod]
    public async Task FanboxClient_UsesCustomUserAgent()
    {
        var handler = new TestHttpMessageHandler();
        handler.When("https://api.fanbox.cc/plan.listSupporting", () => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"body\":[]}")
        });

        var client = new FanboxClient("__cf_bm=xxx;cf_clearance=yyy;FANBOXSESSID=zzz;", handler, CustomUA);
        await client.GetSupportingPlansAsync();

        Assert.AreEqual(CustomUA, handler.LastRequest?.Headers.UserAgent.ToString());
    }
}
