using System.Text.Json;
using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.SerializerContexts;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Novel series (without chapter information)
/// </summary>
/// <param name="Id">Series id</param>
/// <param name="UserId">Author uid</param>
/// <param name="UserName">Author nickname</param>
/// <param name="UserProfileImageUrl">Author avatar image</param>
/// <param name="XRestrict">Restriction level</param>
/// <param name="IsOriginal">Is original</param>
/// <param name="IsConcluded">Is concluded</param>
/// <param name="Title">Series title</param>
/// <param name="Caption">Caption</param>
/// <param name="Language">Language, not necessarily reliable</param>
/// <param name="PublishedContentCount">Chapter count</param>
/// <param name="PublishedTotalCharacterCount">Total character count</param>
/// <param name="PublishedTotalWordCount">Total word count</param>
/// <param name="PublishedReadingTime">Total reading time, in seconds</param>
/// <param name="LastPublishedContentTimestamp">Timestamp of the last published chapter</param>
/// <param name="CreatedTimestamp">Creation timestamp</param>
/// <param name="UpdatedTimestamp">Upload timestamp</param>
/// <param name="CreateDate">Creation time</param>
/// <param name="UpdateDate">Upload time</param>
/// <param name="FirstNovelId">Novel ID of the first chapter</param>
/// <param name="LatestNovelId">Novel ID of the last chapter</param>
/// <param name="DisplaySeriesContentCount">Displayable chapter count</param>
/// <param name="Total">Chapter count</param>
/// <param name="IsWatched">Followed</param>
/// <param name="IsNotifying">Follow notifications enabled</param>
/// <param name="CoverUrls">Covers of different sizes</param>
public record NovelSeries(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("profileImageUrl")]
    string UserProfileImageUrl,

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("isOriginal")]
    bool IsOriginal,

    [property: JsonPropertyName("isConcluded")]
    bool IsConcluded,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("caption")]
    string Caption,

    [property: JsonPropertyName("language")]
    string Language,

    [property: JsonPropertyName("publishedContentCount")]
    int PublishedContentCount,

    [property: JsonPropertyName("publishedTotalCharacterCount")]
    int PublishedTotalCharacterCount,

    [property: JsonPropertyName("publishedTotalWordCount")]
    int PublishedTotalWordCount,

    [property: JsonPropertyName("publishedReadingTime")]
    int PublishedReadingTime,

    [property: JsonPropertyName("lastPublishedContentTimestamp")]
    int LastPublishedContentTimestamp,

    [property: JsonPropertyName("createdTimestamp")]
    int CreatedTimestamp,

    [property: JsonPropertyName("updatedTimestamp")]
    int UpdatedTimestamp,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("updateDate")]
    DateTimeOffset UpdateDate,

    [property: JsonPropertyName("firstNovelId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int FirstNovelId,

    [property: JsonPropertyName("latestNovelId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int LatestNovelId,

    [property: JsonPropertyName("displaySeriesContentCount")]
    int DisplaySeriesContentCount,

    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("isWatched")]
    bool IsWatched,

    [property: JsonPropertyName("isNotifying")]
    bool IsNotifying,

    [property: JsonPropertyName("cover")]
    [property: JsonConverter(typeof(NovelSeriesCoverJsonConverter))]
    NovelImageUrls CoverUrls
);


internal class NovelSeriesCoverJsonConverter : JsonConverter<NovelImageUrls>
{
    public override NovelImageUrls? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var wrapper = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.NovelSeriesCoverUrlsWrapper);
        return wrapper?.Urls;
    }

    public override void Write(Utf8JsonWriter writer, NovelImageUrls value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(new NovelSeriesCoverUrlsWrapper(value), PixivJsonSerializerContext.Default.NovelSeriesCoverUrlsWrapper));
    }


    /// <summary>
    /// Cover image wrapper
    /// </summary>
    /// <param name="Urls">Covers of different sizes</param>
    public record NovelSeriesCoverUrlsWrapper(
        [property: JsonPropertyName("urls")]
        NovelImageUrls Urls
    );
}