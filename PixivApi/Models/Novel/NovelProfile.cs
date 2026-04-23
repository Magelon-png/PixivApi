using Scighost.PixivApi.Models.Common;
using System.Text.Json.Serialization;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Simple information for novels (without content)
/// </summary>
/// <param name="Id">Novel id</param>
/// <param name="Title">Title</param>
/// <param name="XRestrict">Restriction level</param>
/// <param name="Url">Cover image</param>
/// <param name="Tags">Tags</param>
/// <param name="UserId">Author uid</param>
/// <param name="UserName">Author nickname</param>
/// <param name="TextCount">Text count</param>
/// <param name="WordCount">Word count</param>
/// <param name="ReadingTime">Reading time, in seconds</param>
/// <param name="Description">Description, HTML format</param>
/// <param name="CreateDate">Creation time</param>
/// <param name="UpdateDate">Upload time</param>
/// <param name="SeriesId">Series id</param>
/// <param name="SeriesTitle">Series title</param>
/// <param name="UserProfileImageUrl">Author avatar image</param>
public record NovelProfile(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("url")]
    string Url,

    [property: JsonPropertyName("tags")]
    [property: JsonConverter(typeof(NovelTagsJsonConverter))]
    List<string> Tags,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("textCount")]
    int TextCount,

    [property: JsonPropertyName("wordCount")]
    int WordCount,

    [property: JsonPropertyName("readingTime")]
    int ReadingTime,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("updateDate")]
    DateTimeOffset UpdateDate,

    [property: JsonPropertyName("seriesId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int SeriesId,

    [property: JsonPropertyName("seriesTitle")]
    string SeriesTitle,

    [property: JsonPropertyName("profileImageUrl")]
    string? UserProfileImageUrl
);
