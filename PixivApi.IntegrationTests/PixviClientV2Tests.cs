using System.Diagnostics;
using Microsoft.Playwright;
using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.V2.Illust;

namespace PixivApi.IntegrationTests;

[TestClass]
public class PixviClientV2Tests
{
    private static PixivClientV2 SharedClient;

    [ClassInitialize]
    public static async Task ClassInit(TestContext context)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = false
        });
        var page = await browser.NewPageAsync(new BrowserNewPageOptions()
        {
            ViewportSize = new ViewportSize()
            {
                Width = 1920,
                Height = 1080
            }
            ,Locale = "en-GB"
        });
        var client = new PixivClientV2();
        var url = client.GetCodeLoginUrl();
        string token = null;
        page.Request += (_, request) =>
        {
            if (request.Url.StartsWith("https://app-api.pixiv.net/web/v1/users/auth/pixiv/callback"))
            {
                token = request.Url.Split("code=").Last().Split("&via=").First();
            }
        };
        await page.GotoAsync(url);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var fieldsets = await page.Locator("fieldset > label > input").AllAsync();
        Assert.AreEqual(2, fieldsets.Count);
        //Get input inside fieldSet
        var loginInput = fieldsets[0];
        await loginInput.ClickAsync();
        await loginInput.PressSequentiallyAsync(Environment.GetEnvironmentVariable("PIXIV_USERNAME"));
        var passwordInput = fieldsets.Last();
        await passwordInput.ClickAsync();
        await passwordInput.PressSequentiallyAsync(Environment.GetEnvironmentVariable("PIXIV_PASSWORD"));
        var loginButton = page.GetByRole(AriaRole.Button, new() { Name = "Log In", Exact = true });
        await loginButton.ClickAsync();
        var retries = 0;
        while (token is null && retries < 10)
        {
            await Task.Delay(5000);
            retries++;
        }

        if (token is null)
        {
            throw new InvalidOperationException("Token is null. Failed to retrieve the callback token");
        }
        await client.LoginAsync(token);
        await client.RefreshTokenAsync();
        SharedClient = client;
        await page.CloseAsync();
    }

    [ClassCleanup]
    public static void Cleanup()
    {
        SharedClient?.Dispose();
    }

    [TestMethod]
    public async Task GetIllustsRanking()
    {
        var illusts = await SharedClient.GetIllustsRanking(RankingMode.Month);
        Assert.AreEqual(18, illusts.Illusts.Count);
    }
    
    [TestMethod]
    [DataRow(24367326)]
    public async Task GetUserIllustAsync(int userId)
    {
        var illusts = await SharedClient.GetUserIllustsAsync(userId);
        Assert.HasCount(30, illusts.Illusts);
    }
    
    [TestMethod]
    [DataRow(56099861, "春の到来")]
    [DataRow(57826890, "夏、吹いてくる風")]
    public async Task GetIllustDetailsAsync(int illustId, string title)
    {
        var illust = await SharedClient.GetIllustDetailsAsync(illustId);
        Assert.AreEqual(title, illust.Illust.Title);
    }

    [TestMethod]
    [DataRow(49319675, 105)]
    public async Task GetUgoiraMetadataAsync(int illustId, int frameCount)
    {
        var ugoiraMeta = await SharedClient.GetUgoiraMetadataAsync(illustId);
        Assert.HasCount(frameCount, ugoiraMeta.UgoiraMetadata.Frames);
    }
    
    // [TestMethod]
    // [DataRow(49319675, 5)]
    // public async Task GetIllustCommentsAsync(int illustId, int minCommentCount)
    // {
    //     var comments = await SharedClient.GetIllustCommentsAsync(illustId);
    //     Assert.IsGreaterThanOrEqualTo(minCommentCount, comments.Comments.Count, $"Expected at least {minCommentCount} comments, but got {comments.Comments.Count}");
    // }
    
    [TestMethod]
    [DataRow(49319675)]
    public async Task GetRelatedIllusts(int illustId)
    {
        var relatedIllusts = await SharedClient.GetRelatedIllusts(illustId);
        Assert.HasCount(30, relatedIllusts.Illusts);
    }

    [TestMethod]
    public async Task GetFollowIllustrationsAsync()
    {
        var illustrations = await SharedClient.GetFollowIllustrationsAsync();
        Assert.HasCount(30, illustrations.Illusts);
    }
    
    [TestMethod]
    //Should have bookmarked illustId to run this test
    [DataRow(143973160)]
    public async Task GetIllustBookmarkDetailAsync(int illustId)
    {
        var details = await SharedClient.GetIllustBookmarkDetailAsync(illustId);
        Assert.IsTrue(details.BookmarkDetail.IsBookmarked);
    }

    [TestMethod]
    public async Task GetNewIllustrationsAsync()
    {
        var illustrations = await SharedClient.GetNewIllustrationAsync(IllustrationContentType.Illust);
        Assert.IsNotNull(illustrations.Illusts);
        Assert.HasCount(30, illustrations.Illusts);
        Assert.IsNotNull(illustrations.NextUrl);
    }
    // Endpoint does not exists
    // [TestMethod]
    // public async Task GetPopularIllustrationAsync()
    // {
    //     var illustrations = await SharedClient.GetPopularIllustrationAsync(IllustrationContentType.Illust);
    //     Assert.IsNotNull(illustrations.Illusts);
    //     Assert.HasCount(30, illustrations.Illusts);
    //     Assert.IsNotNull(illustrations.NextUrl);
    // }

    [TestMethod]
    public async Task GetRecommendedIllustrations()
    {
        var illustrations = await SharedClient.GetRecommendedIllustrations(IllustrationContentType.Illust);
        Assert.IsNotNull(illustrations.Illusts);
        Assert.IsGreaterThan(30, illustrations.Illusts.Count);
        Assert.IsNotNull(illustrations.NextUrl);
    }

    [TestMethod]
    [DataRow("Miku")]
    public async Task SearchIllustsAsync(string searchTerm)
    {
        var illustrations = await SharedClient.SearchIllustsAsync(searchTerm, SearchOrderV2.DateDescending);
        Assert.IsNotNull(illustrations.Illusts);
        Assert.HasCount(30, illustrations.Illusts);
        Assert.IsNotNull(illustrations.NextUrl);
    }
}