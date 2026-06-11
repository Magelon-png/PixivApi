using System.Net;
using System.Text;
using Scighost.PixivApi;
using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.Search;
using Scighost.PixivApi.Models.Illust;

namespace PixivApi.Tests;

[TestClass]
public sealed class SearchTests
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

    private const string Payload = "Illust/SearchIllustrations.json";

    [TestMethod]
    public async Task SearchAsync_SingleKeyword()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/%E3%83%A1%E3%82%A4%E3%83%89?word=%E3%83%A1%E3%82%A4%E3%83%89&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["メイド"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_EnglishKeyword()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_MultipleKeywords()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/hello%20world?word=hello+world&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag&type=illust_and_ugoira&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["hello", "world"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_KeywordWithSpace()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/hello%20world?word=hello+world&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag&type=illust_and_ugoira&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["hello world"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    [DataRow(SearchAge.AllAges, "safe")]
    [DataRow(SearchAge.R18, "r18")]
    public async Task SearchAsync_AgeFilter(SearchAge age, string expectedMode)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode={expectedMode}&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            age, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    [DataRow(SearchOrder.DateAscending, "date")]
    [DataRow(SearchOrder.PopularityDescending, "popular_d")]
    [DataRow(SearchOrder.PopularityAscending, "popular")]
    public async Task SearchAsync_OrderBy(SearchOrder order, string expectedOrder)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/search/illustrations/test?word=test&order={expectedOrder}&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], order,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    [DataRow(SearchTarget.Any, "all")]
    [DataRow(SearchTarget.Illust, "illust")]
    [DataRow(SearchTarget.Manga, "manga")]
    [DataRow(SearchTarget.Ugoira, "ugoira")]
    public async Task SearchAsync_Target(SearchTarget target, string expectedType)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type={expectedType}&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, target, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_HideAi()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&ai_type=1&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English, hideAi: true);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_WithoutLanguage()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, lang: null);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_DateRange()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en&scd=2024-01-01&ecd=2024-12-31",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English,
            notBeforeDate: new DateOnly(2024, 1, 1), notAfterDate: new DateOnly(2024, 12, 31));

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_BookmarkDates()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en&sbd=2024-06-01&ebd=2024-08-01",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English,
            bookmarkedAfterDate: new DateOnly(2024, 6, 1), bookmarkedBeforeDate: new DateOnly(2024, 8, 1));

        Assert.IsNotNull(result);
    }

    [TestMethod]
    [DataRow(BookmarkVisibilityFilter.All, "1")]
    [DataRow(BookmarkVisibilityFilter.Public, "2")]
    [DataRow(BookmarkVisibilityFilter.Private, "3")]
    public async Task SearchAsync_BookmarkVisibility(BookmarkVisibilityFilter filter, string expectedWib)
    {
        _handler.When(
            $"https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en&wib={expectedWib}",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English,
            bookmarkVisibilityFilter: filter);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_BookmarkCount()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=1&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en&blt=1000&bgt=5000",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(1, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English,
            minimumBookmarkCount: 1000, maximumBookmarkCount: 5000);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_Page()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=date_d&mode=all&p=3&csw=0&s_mode=s_tag_full&type=illust_and_ugoira&lang=en",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(3, ["test"], SearchOrder.DateDescending,
            SearchAge.AnyAge, SearchTarget.IllustAndUgoira, true, SearchLanguage.English);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_ComplexCombination()
    {
        _handler.When(
            "https://www.pixiv.net/ajax/search/illustrations/test?word=test&order=popular_d&mode=r18&p=2&csw=0&s_mode=s_tag_full&type=manga&ai_type=1&lang=en&scd=2024-01-01&ecd=2024-12-31&wib=2&sbd=2024-06-01&ebd=2024-08-01&blt=1000&bgt=5000",
            () => OkJson(Payload));

        var result = await _pixivClient.SearchIllustrationsAsync(2, ["test"], SearchOrder.PopularityDescending,
            SearchAge.R18, SearchTarget.Manga, true, SearchLanguage.English, hideAi: true,
            notBeforeDate: new DateOnly(2024, 1, 1), notAfterDate: new DateOnly(2024, 12, 31),
            bookmarkVisibilityFilter: BookmarkVisibilityFilter.Public,
            bookmarkedAfterDate: new DateOnly(2024, 6, 1), bookmarkedBeforeDate: new DateOnly(2024, 8, 1),
            minimumBookmarkCount: 1000, maximumBookmarkCount: 5000);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAsync_EmptyKeywords_Throws()
    {
        try
        {
            await _pixivClient.SearchIllustrationsAsync(1, [], SearchOrder.DateDescending);
            Assert.Fail("Expected ArgumentException was not thrown");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
