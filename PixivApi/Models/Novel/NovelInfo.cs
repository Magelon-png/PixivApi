using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Detailed information for novels (including content)
/// </summary>
/// <param name="Id">Novel id</param>
/// <param name="Title">Novel title</param>
/// <param name="Description">Description, HTML format</param>
/// <param name="BookmarkCount">Bookmark count</param>
/// <param name="CommentCount">Comment count</param>
/// <param name="MarkerCount">Marker count</param>
/// <param name="CreateDate">Creation time</param>
/// <param name="UploadDate">Upload time</param>
/// <param name="LikeCount">Like count</param>
/// <param name="PageCount">Number of pages in the novel, in <see cref="Content"/>, separated by "[newpage]"</param>
/// <param name="UserId">Author uid</param>
/// <param name="UserName">Author nickname</param>
/// <param name="ViewCount">View count</param>
/// <param name="XRestrict">Restriction level</param>
/// <param name="Content">Content, known special formats include [newpage], [chapter:{chapterName}], [uploadedimage:{imageId}]</param>
/// <param name="CoverUrl">Cover image</param>
/// <param name="IsLike">Liked</param>
/// <param name="Tags">Tags</param>
/// <param name="SeriesNavData">Sidebar data for novel reading pages</param>
/// <param name="UserNovels">IDs of all novels by the author</param>
/// <param name="Language">Language, not necessarily reliable</param>
/// <param name="CharacterCount">Character count</param>
/// <param name="WordCount">Word count</param>
/// <param name="ReadingTime">Reading time, in seconds</param>
/// <param name="IsOriginal">Is original</param>
/// <param name="BookmarkData">Bookmark information, null if not bookmarked</param>
/// <param name="Marker">Which page has a marker, null if no marker</param>
/// <param name="TextEmbeddedImages">Images embedded in the novel content, format is [uploadedimage:{id}]</param>
public record NovelInfo(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("bookmarkCount")]
    int BookmarkCount,

    [property: JsonPropertyName("commentCount")]
    int CommentCount,

    [property: JsonPropertyName("markerCount")]
    int MarkerCount,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("uploadDate")]
    DateTimeOffset UploadDate,

    [property: JsonPropertyName("likeCount")]
    int LikeCount,

    [property: JsonPropertyName("pageCount")]
    int PageCount,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("viewCount")]
    int ViewCount,

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("content")]
    string Content,

    [property: JsonPropertyName("coverUrl")]
    string CoverUrl,

    [property: JsonPropertyName("likeData")]
    bool IsLike,

    [property: JsonPropertyName("tags")]
    [property: JsonConverter(typeof(PixivTagJsonConverter))]
    List<PixivTag> Tags,

    [property: JsonPropertyName("seriesNavData")]
    SeriesNavData? SeriesNavData,

    [property: JsonPropertyName("userNovels")]
    [property: JsonConverter(typeof(DictionaryKeyToListJsonConverterInt32))]
    List<int> UserNovels,

    [property: JsonPropertyName("language")]
    string Language,

    [property: JsonPropertyName("characterCount")]
    int CharacterCount,

    [property: JsonPropertyName("wordCount")]
    int WordCount,

    [property: JsonPropertyName("readingTime")]
    int ReadingTime,

    [property: JsonPropertyName("isOriginal")]
    bool IsOriginal,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData,

    [property: JsonPropertyName("marker")]
    int? Marker,

    [property: JsonPropertyName("textEmbeddedImages")]
    Dictionary<int, TextEmbeddedImage> TextEmbeddedImages
);

