namespace Scighost.PixivApi.Models.Illust;

internal sealed record DeleteBookmarkIllustsRequest(
    [property: JsonPropertyName("bookmarkIds")] IEnumerable<string> BookmarkIds);