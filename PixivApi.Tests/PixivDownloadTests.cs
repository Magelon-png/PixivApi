using Scighost.PixivApi.Clients;

namespace PixivApi.Tests;

[TestClass]
public sealed class PixivDownloadTests
{
    private TestHttpMessageHandler _handler = null!;
    private PixivClient _pixivClient = null!;

    [TestInitialize]
    public void Initialize()
    {
        _handler = new TestHttpMessageHandler();
        _pixivClient = new PixivClient(cfBm: "xxx", cfClearance: "yyy", phpsessid: "zzz", clientHandler: _handler);
    }

    [TestMethod]
    public async Task DownloadIllustAsync_RetryDoesNotAppendPartialData()
    {
        const string url = "https://i.pximg.net/img-original/test-image.jpg";
        var partialAttempt = DownloadTestHelpers.CreatePayload(0xAA, 4096);
        var successfulAttempt = DownloadTestHelpers.CreatePayload(0xBB, 1024);
        _handler.ConfigureFlakyDownload(url, partialAttempt, failAfterBytes: 2048, successfulAttempt);

        using var destination = new MemoryStream();

        var downloaded = await _pixivClient.DownloadIllustAsync(url, destination, CancellationToken.None);

        Assert.IsTrue(downloaded);
        CollectionAssert.AreEqual(successfulAttempt, destination.ToArray());
    }

    [TestMethod]
    public async Task DownloadIllustAsync_MultiplePartialAttempts_DoesNotAppendPartialData()
    {
        const string url = "https://i.pximg.net/img-original/test-image-multiple-partials.jpg";
        var partialAttempt = DownloadTestHelpers.CreatePayload(0xAA, 8192);
        var successfulAttempt = DownloadTestHelpers.CreatePayload(0xBB, 1024);
        _handler.ConfigureFlakyDownload(
            url,
            partialAttempt,
            failAfterBytes: 4096,
            successfulAttempt,
            failingAttempts: 2);

        using var destination = new MemoryStream();

        var downloaded = await _pixivClient.DownloadIllustAsync(url, destination, CancellationToken.None);

        Assert.IsTrue(downloaded);
        CollectionAssert.AreEqual(successfulAttempt, destination.ToArray());
    }

    [TestMethod]
    public async Task DownloadIllustAsync_NonSeekableDestination_RetryDoesNotAppendPartialData()
    {
        const string url = "https://i.pximg.net/img-original/test-image-nonseekable.jpg";
        var partialAttempt = DownloadTestHelpers.CreatePayload(0xAA, 4096);
        var successfulAttempt = DownloadTestHelpers.CreatePayload(0xBB, 1024);
        _handler.ConfigureFlakyDownload(url, partialAttempt, failAfterBytes: 2048, successfulAttempt);

        using var inner = new MemoryStream();
        using var destination = new NonSeekableWriteStream(inner);

        var downloaded = await _pixivClient.DownloadIllustAsync(url, destination, CancellationToken.None);

        Assert.IsTrue(downloaded);
        CollectionAssert.AreEqual(successfulAttempt, inner.ToArray());
    }

    [TestCleanup]
    public void Cleanup()
    {
        _pixivClient.Dispose();
    }
}
