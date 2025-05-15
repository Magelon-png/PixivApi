namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Ja"></param>
/// <param name="En"></param>
public record IllustSearchAlternateLanguages(
    [property: JsonPropertyName("ja")] string Ja,
    [property: JsonPropertyName("en")] string En
);