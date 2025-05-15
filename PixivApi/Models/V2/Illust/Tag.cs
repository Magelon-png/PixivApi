namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Name"></param>
/// <param name="TranslatedName"></param>
public record Tag(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("translated_name")]
    string TranslatedName
);