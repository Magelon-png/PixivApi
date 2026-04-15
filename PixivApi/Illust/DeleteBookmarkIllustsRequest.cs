namespace Scighost.PixivApi.Illust;

internal record DeleteBookmarkIllustsRequest(
    [property: JsonPropertyName("bookmarkIds")] IEnumerable<string> BookmarkIds);