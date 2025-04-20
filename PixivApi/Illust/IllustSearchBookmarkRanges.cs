namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Min"></param>
/// <param name="Max"></param>
public record IllustSearchBookmarkRanges(
    [property: JsonPropertyName("min")] int? Min,
    [property: JsonPropertyName("max")] int? Max
);