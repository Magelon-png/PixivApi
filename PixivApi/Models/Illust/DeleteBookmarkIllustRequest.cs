namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="BookmarkId"></param>
public record DeleteBookmarkIllustRequest(
    [property:JsonPropertyName("bookmark_id")]
    [property:JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    long BookmarkId);