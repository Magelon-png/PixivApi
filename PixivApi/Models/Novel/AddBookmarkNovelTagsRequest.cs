using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Add bookmark novel tags request
/// </summary>
/// <param name="BookmarkIds">Bookmark ids</param>
/// <param name="Tags">Tags to add</param>
public record AddBookmarkNovelTagsRequest(
    [property: JsonPropertyName("bookmarkIds")]
    IEnumerable<string> BookmarkIds,

    [property: JsonPropertyName("tags")]
    IEnumerable<string> Tags
);
