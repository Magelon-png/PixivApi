namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Meta"></param>
public record IllustSearchExtraData(
    [property: JsonPropertyName("meta")] IllustSearchMeta Meta
);