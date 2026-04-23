namespace Scighost.PixivApi.Models.Illust;

internal sealed record AddBookmarkIllustTagsRequest(
    [property: JsonPropertyName("bookmarkIds")] IEnumerable<string> BookmarkIds,
    [property: JsonPropertyName("tags")] IEnumerable<string> Tags
);