namespace Scighost.PixivApi.Models.Illust;

internal record ChangeBookmarkIllustVisibilityRequest(
    [property: JsonPropertyName("bookmarkIds")] IEnumerable<string> BookmarkIds,
    [property: JsonPropertyName("bookmarkRestrict")] string BookmarkRestrict
);
