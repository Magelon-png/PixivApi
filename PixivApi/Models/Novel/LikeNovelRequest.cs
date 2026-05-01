using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Like novel request
/// </summary>
/// <param name="NovelId">Novel id</param>
public record LikeNovelRequest(
    [property: JsonPropertyName("novel_id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int NovelId
);
