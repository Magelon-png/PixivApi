using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;


/// <summary>
/// Simple information for illustrations and manga
/// </summary>
/// <param name="Id">Work id</param>
/// <param name="Title">Work title</param>
/// <param name="IllustType">Work type, illustration or manga</param>
/// <param name="XRestrict">Restriction level</param>
/// <param name="Url">Address of the first image</param>
/// <param name="Description">Description, HTML format</param>
/// <param name="Tags">Original work tags, untranslated</param>
/// <param name="UserId">Author uid</param>
/// <param name="UserName">Author nickname</param>
/// <param name="Width">Pixel width of the first image</param>
/// <param name="Height">Pixel height of the first image</param>
/// <param name="PageCount">Number of images in the work</param>
/// <param name="BookmarkData">Bookmark information, null if not bookmarked</param>
/// <param name="CreateDate">Creation time</param>
/// <param name="UpdateDate">Upload time</param>
/// <param name="UserProfileImageUrl">User avatar image address</param>
public record IllustProfile(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("illustType")]
    IllustType IllustType,

    [property: JsonPropertyName("xRestrict")]
    int XRestrict,

    [property: JsonPropertyName("url")]
    string Url,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("tags")]
    List<string> Tags,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("width")]
    int Width,

    [property: JsonPropertyName("height")]
    int Height,

    [property: JsonPropertyName("pageCount")]
    int PageCount,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("updateDate")]
    DateTimeOffset UpdateDate,

    [property: JsonPropertyName("profileImageUrl")]
    string UserProfileImageUrl
);

/// <inheritdoc/>
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="IllustType"></param>
/// <param name="XRestrict"></param>
/// <param name="Url"></param>
/// <param name="Description"></param>
/// <param name="Tags"></param>
/// <param name="UserId"></param>
/// <param name="UserName"></param>
/// <param name="Width"></param>
/// <param name="Height"></param>
/// <param name="PageCount"></param>
/// <param name="BookmarkData"></param>
/// <param name="CreateDate"></param>
/// <param name="UpdateDate"></param>
/// <param name="UserProfileImageUrl"></param>
/// <param name="SeriesId"></param>
/// <param name="SeriesTitle"></param>
/// <param name="AlternativeTitle"></param>
/// <param name="IsBookmarkable"></param>
public record MangaProfile(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("illustType")]
    IllustType IllustType,

    [property: JsonPropertyName("xRestrict")]
    int XRestrict,

    [property: JsonPropertyName("url")]
    string Url,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("tags")]
    List<string> Tags,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("width")]
    int Width,

    [property: JsonPropertyName("height")]
    int Height,

    [property: JsonPropertyName("pageCount")]
    int PageCount,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("updateDate")]
    DateTimeOffset UpdateDate,

    [property: JsonPropertyName("profileImageUrl")]
    string UserProfileImageUrl,
    [property: JsonPropertyName("seriesId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int SeriesId,
    [property: JsonPropertyName("seriesTitle")]
    string SeriesTitle,
    [property: JsonPropertyName("alt")]
    string AlternativeTitle,
    [property: JsonPropertyName("isBookmarkable")]
    bool IsBookmarkable
    
) : IllustProfile(Id, Title, IllustType, XRestrict, Url, Description, Tags, UserId, UserName, Width, Height, PageCount, BookmarkData, CreateDate, UpdateDate, UserProfileImageUrl);