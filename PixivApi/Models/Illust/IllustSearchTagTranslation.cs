namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="EnglishTranslation"></param>
public record IllustSearchTagTranslation(
    [property: JsonPropertyName("en")] string EnglishTranslation
);