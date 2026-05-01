namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="BookmarkId"></param>
public record AddBookmarkIllustResponse(
    [property: JsonPropertyName("last_bookmark_id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    long BookmarkId);