namespace Scighost.PixivApi.Models.Illust;


/// <summary>
/// Manga series
/// </summary>
/// <param name="Id">Manga series ID</param>
/// <param name="UserId">Author uid</param>
/// <param name="Title">Manga series title</param>
/// <param name="Description">Description</param>
/// <param name="Caption">Caption</param>
/// <param name="Total">Chapter count</param>
/// <param name="Url">Manga series cover image</param>
/// <param name="FirstIllustId">Illustration ID of the first chapter</param>
/// <param name="LatestIllustId">Illustration ID of the latest chapter</param>
/// <param name="CreateDate">Creation time</param>
/// <param name="UpdateDate">Update time</param>
/// <param name="IsWatched">Followed</param>
/// <param name="IsNotifying">Follow notifications enabled</param>
/// <param name="Illusts">Content of the manga series</param>
public record MangaSeries(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("caption")]
    string Caption,

    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("url")]
    string Url,

    [property: JsonPropertyName("firstIllustId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int FirstIllustId,

    [property: JsonPropertyName("latestIllustId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int LatestIllustId,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("updateDate")]
    DateTimeOffset UpdateDate,

    [property: JsonPropertyName("isWatched")]
    bool IsWatched,

    [property: JsonPropertyName("isNotifying")]
    bool IsNotifying,

    List<MangaSeriesIllust> Illusts
)
{
    /// <summary>
    /// Checks if the first chapter was returned when requesting for chapters. Otherwise, more pages exists.
    /// </summary>
    /// <returns></returns>
    public bool HasNextPage() => Total > Illusts.Count 
                                 && Illusts.All(i => i.Order != 1);
}


/// <summary>
/// Content of a manga series
/// </summary>
/// <param name="IllustId">Manga ID</param>
/// <param name="Order">Sort order in the series</param>
/// <param name="IllustProfile">Manga information</param>
public record MangaSeriesIllust(
    int IllustId,
    int Order,
    IllustProfile IllustProfile
);