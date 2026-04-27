using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Change bookmark novel visibility request
/// </summary>
/// <param name="BookmarkIds">Bookmark ids</param>
/// <param name="BookmarkRestrict">Visibility: "private" or "public"</param>
public record ChangeBookmarkNovelVisibilityRequest(
    [property: JsonPropertyName("bookmarkIds")]
    IEnumerable<string> BookmarkIds,

    [property: JsonPropertyName("bookmarkRestrict")]
    string BookmarkRestrict
);
