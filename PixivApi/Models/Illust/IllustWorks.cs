namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Works"></param>
public record IllustWorks(
    [property: JsonPropertyName("works")]
    Dictionary<string, IllustWorksInfo> Works
);