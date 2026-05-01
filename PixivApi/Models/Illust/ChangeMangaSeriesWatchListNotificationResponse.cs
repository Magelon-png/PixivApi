using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
///
/// </summary>
/// <param name="Notifiable"></param>
[JsonConverter(typeof(ChangeMangaSeriesWatchListNotificationResponseJsonConverter))]
public record ChangeMangaSeriesWatchListNotificationResponse(
    [property: JsonPropertyName("notifiable")] bool? Notifiable
    );