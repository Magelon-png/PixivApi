namespace Scighost.PixivApi.Models.Common;

/// <summary>
/// All custom tags of user's bookmarked works. If there are too many tags, not all will be included.
/// </summary>
/// <param name="Public"></param>
/// <param name="Private"></param>
/// <param name="TooManyBookmark"></param>
/// <param name="TooManyBookmarkTags"></param>
public record UserBookmarkTag(
    [property: JsonPropertyName("public")]
    List<BookmarkTag> Public,
    [property: JsonPropertyName("private")]
    List<BookmarkTag> Private,
    [property: JsonPropertyName("tooManyBookmark")]
    bool TooManyBookmark,
    [property: JsonPropertyName("tooManyBookmarkTags")]
    bool TooManyBookmarkTags
);


/// <summary>
/// Custom tag name of bookmarked works and the corresponding number of works
/// </summary>
/// <param name="Name"></param>
/// <param name="Count"></param>
public record BookmarkTag(
    [property: JsonPropertyName("tag")]
    string Name,
    [property: JsonPropertyName("cnt")]
    int Count
);