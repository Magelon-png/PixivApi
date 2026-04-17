namespace Scighost.PixivApi.Models.Common;


/// <summary>
/// Sidebar series data when reading manga or novels
/// </summary>
/// <param name="SeriesType">Manga or novel, "manga" or "novel"</param>
/// <param name="SeriesId">Work series id</param>
/// <param name="Title">Manga or novel series name</param>
/// <param name="IsConcluded">Is concluded</param>
/// <param name="IsReplaceable">Is replaceable</param>
/// <param name="IsWatched">Followed</param>
/// <param name="IsNotifying">Added to follow notification list</param>
/// <param name="Order">Position of the work in this series</param>
/// <param name="Preview">Previous work in the series</param>
/// <param name="Next">Next work in the series</param>
public record SeriesNavData(
    [property: JsonPropertyName("seriesType")]
    string SeriesType,

    [property: JsonPropertyName("seriesId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int SeriesId,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("isConcluded")]
    bool IsConcluded,

    [property: JsonPropertyName("isReplaceable")]
    bool IsReplaceable,

    [property: JsonPropertyName("isWatched")]
    bool IsWatched,

    [property: JsonPropertyName("isNotifying")]
    bool IsNotifying,

    [property: JsonPropertyName("order")]
    int Order,

    [property: JsonPropertyName("prev")]
    SeriesNavDataPreviewOrNextData? Preview,

    [property: JsonPropertyName("next")]
    SeriesNavDataPreviewOrNextData? Next
);


/// <summary>
/// Previous or next work in a series
/// </summary>
/// <param name="Available">Available</param>
/// <param name="Id">Work id</param>
/// <param name="Order">Position of the work in this series</param>
/// <param name="Title">Title</param>
public record SeriesNavDataPreviewOrNextData(
    [property: JsonPropertyName("available")]
    bool Available,

    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("order")]
    int Order,

    [property: JsonPropertyName("title")]
    string Title
);