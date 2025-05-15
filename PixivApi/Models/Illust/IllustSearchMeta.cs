namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Canonical"></param>
/// <param name="AlternateLanguages"></param>
/// <param name="DescriptionHeader"></param>
public record IllustSearchMeta(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("canonical")] string Canonical,
    [property: JsonPropertyName("alternateLanguages")] IllustSearchAlternateLanguages AlternateLanguages,
    [property: JsonPropertyName("descriptionHeader")] string DescriptionHeader
);