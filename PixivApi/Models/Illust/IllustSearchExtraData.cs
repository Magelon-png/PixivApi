namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Meta"></param>
public record IllustSearchExtraData(
    [property: JsonPropertyName("meta")] IllustSearchMeta Meta
);