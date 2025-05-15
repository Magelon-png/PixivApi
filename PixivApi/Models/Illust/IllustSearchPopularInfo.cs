namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Recent"></param>
/// <param name="Permanent"></param>
public record IllustSearchPopularInfo(
    [property: JsonPropertyName("recent")] IllustSearchData[] Recent,
    [property: JsonPropertyName("permanent")] IllustSearchData[] Permanent
);