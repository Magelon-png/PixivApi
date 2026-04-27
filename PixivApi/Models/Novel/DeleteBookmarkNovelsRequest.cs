using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Delete bookmark novels request
/// </summary>
/// <param name="BookmarkIds">Bookmark ids</param>
public record DeleteBookmarkNovelsRequest(
    [property: JsonPropertyName("bookmarkIds")]
    IEnumerable<string> BookmarkIds
);
