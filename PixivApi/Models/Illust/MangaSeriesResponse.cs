using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

internal sealed class MangaSeriesResponse
{
    [JsonPropertyName("thumbnails")]
    public Thumbnails Thumbnails { get; set; }

    [JsonPropertyName("illustSeries")]
    public List<MangaSeries> MangaSeries { get; set; }

    [JsonPropertyName("page")]
    public IllustSeriesWorkWrapper Page { get; set; }

    public MangaSeriesResponse(Thumbnails thumbnails, List<MangaSeries> mangaSeries, IllustSeriesWorkWrapper page)
    {
        Thumbnails = thumbnails;
        MangaSeries = mangaSeries;
        Page = page;
    }
}


internal sealed class IllustSeriesWorkWrapper
{
    [JsonPropertyName("series")]
    public List<IllustSeriesWork> Works { get; set; }

    [JsonPropertyName("isSetCover")]
    public bool IsSetCover { get; set; }

    [JsonPropertyName("seriesId")]
    public int SeriesId { get; set; }

    [JsonPropertyName("otherSeriesId")]
    public string OtherSeriesId { get; set; }

    [JsonPropertyName("recentUpdatedWorkIds")]
    public List<int> RecentUpdatedWorkIds { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("isWatched")]
    public bool IsWatched { get; set; }

    [JsonPropertyName("isNotifying")]
    public bool IsNotifying { get; set; }

    public IllustSeriesWorkWrapper(List<IllustSeriesWork> works, bool isSetCover, int seriesId, string otherSeriesId, List<int> recentUpdatedWorkIds, int total, bool isWatched, bool isNotifying)
    {
        Works = works;
        IsSetCover = isSetCover;
        SeriesId = seriesId;
        OtherSeriesId = otherSeriesId;
        RecentUpdatedWorkIds = recentUpdatedWorkIds;
        Total = total;
        IsWatched = isWatched;
        IsNotifying = isNotifying;
    }
}

internal sealed class IllustSeriesWork
{
    [JsonPropertyName("workId")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int WorkId { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }
}