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