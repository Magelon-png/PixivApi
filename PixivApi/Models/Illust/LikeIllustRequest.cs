using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// Like illustration request
/// </summary>
/// <param name="IllustId">Illustration id</param>
public record LikeIllustRequest(
    [property: JsonPropertyName("illust_id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int IllustId
);
