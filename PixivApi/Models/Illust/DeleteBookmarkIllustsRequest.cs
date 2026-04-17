namespace Scighost.PixivApi.Models.Illust;

internal record DeleteBookmarkIllustsRequest(
    [property: JsonPropertyName("bookmarkIds")] IEnumerable<string> BookmarkIds);