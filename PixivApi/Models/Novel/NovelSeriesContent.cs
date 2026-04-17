using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;


internal record NovelSeriesContentWrapper(
    [property: JsonPropertyName("page")]
    NovelSeriesChapterPages Page
);

/// <summary>
/// 
/// </summary>
/// <param name="SeriesContents"></param>
public record NovelSeriesChapterPages(
    [property: JsonPropertyName("seriesContents")]
    List<NovelSeriesChapter> SeriesContents
);


/// <summary>
/// Chapter information of a novel series (without content)
/// </summary>
/// <param name="Id">Novel id</param>
/// <param name="UserId">Author uid</param>
/// <param name="Title">Title</param>
/// <param name="Tags">Tags</param>
/// <param name="XRestrict">Restriction level</param>
/// <param name="IsOriginal">Is original</param>
/// <param name="TextLength">Character count</param>
/// <param name="CharacterCount">Character count</param>
/// <param name="WordCount">Word count</param>
/// <param name="ReadingTime">Reading time, in seconds</param>
/// <param name="BookmarkCount">Bookmark count</param>
/// <param name="Url">Cover image</param>
/// <param name="UploadTimestamp">Upload timestamp</param>
/// <param name="ReuploadTimestamp">Reupload timestamp</param>
/// <param name="BookmarkData">Bookmark information, null if not bookmarked</param>
public record NovelSeriesChapter(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("tags")]
    List<string> Tags,

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("isOriginal")]
    bool IsOriginal,

    [property: JsonPropertyName("textLength")]
    int TextLength,

    [property: JsonPropertyName("characterCount")]
    int CharacterCount,

    [property: JsonPropertyName("wordCount")]
    int WordCount,

    [property: JsonPropertyName("readingTime")]
    int ReadingTime,

    [property: JsonPropertyName("bookmarkCount")]
    int BookmarkCount,

    [property: JsonPropertyName("url")]
    string Url,

    [property: JsonPropertyName("uploadTimestamp")]
    int UploadTimestamp,

    [property: JsonPropertyName("reuploadTimestamp")]
    int ReuploadTimestamp,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData
);
