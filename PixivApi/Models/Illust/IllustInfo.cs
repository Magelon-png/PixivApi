using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// Detailed information for illustrations and manga, with some useless fields ignored
/// </summary>
/// <param name="Id">Illustration/manga id</param>
/// <param name="Title">Illustration/manga title</param>
/// <param name="Description">Description, HTML format</param>
/// <param name="IllustType">Work type, illustration or manga</param>
/// <param name="CreateDate">Creation date</param>
/// <param name="UploadDate">Upload date</param>
/// <param name="XRestrict">Restriction level</param>
/// <param name="Urls">File addresses of different image sizes for the first page. For more images, call <see cref="PixivClient.GetIllustPagesAsync(int)"/></param>
/// <param name="Tags">Work tags</param>
/// <param name="UserId">Author uid</param>
/// <param name="UserName">Author nickname</param>
/// <param name="UserAccount">Username of the logged-in account (unclear why this is exposed)</param>
/// <param name="UserIllusts">IDs of all illustrations by the author</param>
/// <param name="IsLike">Liked</param>
/// <param name="Width">Pixel width of the first image</param>
/// <param name="Height">Pixel height of the first image</param>
/// <param name="PageCount">Number of images in the work</param>
/// <param name="BookmarkCount">Bookmark count</param>
/// <param name="LikeCount">Like count</param>
/// <param name="CommentCount">Comment count</param>
/// <param name="ResponseCount">Response count</param>
/// <param name="ViewCount">View count</param>
/// <param name="IsOriginal">Is original</param>
/// <param name="SeriesNavData">Sidebar data for manga reading pages</param>
/// <param name="BookmarkData">Bookmark information, null if not bookmarked</param>
public record IllustInfo(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("illustType")]
    IllustType IllustType,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("uploadDate")]
    DateTimeOffset UploadDate,

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("urls")]
    IllustImageUrls Urls,

    [property: JsonPropertyName("tags")]
    [property: JsonConverter(typeof(PixivTagJsonConverter))]
    List<PixivTag> Tags,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("userAccount")]
    string UserAccount,

    [property: JsonPropertyName("userIllusts")]
    [property: JsonConverter(typeof(DictionaryKeyToListJsonConverter<int>))]
    List<int> UserIllusts,

    [property: JsonPropertyName("likeData")]
    bool IsLike,

    [property: JsonPropertyName("width")]
    int Width,

    [property: JsonPropertyName("height")]
    int Height,

    [property: JsonPropertyName("pageCount")]
    int PageCount,

    [property: JsonPropertyName("bookmarkCount")]
    int BookmarkCount,

    [property: JsonPropertyName("likeCount")]
    int LikeCount,

    [property: JsonPropertyName("commentCount")]
    int CommentCount,

    [property: JsonPropertyName("responseCount")]
    int ResponseCount,

    [property: JsonPropertyName("viewCount")]
    int ViewCount,

    [property: JsonPropertyName("isOriginal")]
    bool IsOriginal,

    [property: JsonPropertyName("seriesNavData")]
    SeriesNavData? SeriesNavData,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData
);
