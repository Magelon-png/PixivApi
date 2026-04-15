namespace Scighost.PixivApi.Illust;

internal record AddBookmarkIllustTagsRequest(
    [property: JsonPropertyName("bookmarkIds")] IEnumerable<string> BookmarkIds,
    [property: JsonPropertyName("tags")] IEnumerable<string> Tags
);