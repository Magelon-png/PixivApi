using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="TagTranslation"></param>
/// <param name="Thumbnails"></param>
/// <param name="Requests"></param>
/// <param name="Users"></param>
/// <param name="Page"></param>
/// <param name="BoothItems"></param>
/// <param name="SketchLives"></param>
/// <param name="ZoneConfig"></param>
public record MangaHomePageResponse(
    [property: JsonPropertyName("tagTranslation")] Dictionary<string, TopIllustTagTranslation> TagTranslation,
    [property: JsonPropertyName("thumbnails")] ThumbnailsMangaHomePage Thumbnails,
    [property: JsonPropertyName("requests")] Requests[] Requests,
    [property: JsonPropertyName("users")] Users[] Users,
    [property: JsonPropertyName("page")] Page Page,
    [property: JsonPropertyName("boothItems")] BoothItems[] BoothItems,
    [property: JsonPropertyName("sketchLives")] object[] SketchLives,
    [property: JsonPropertyName("zoneConfig")] ZoneConfig ZoneConfig
);