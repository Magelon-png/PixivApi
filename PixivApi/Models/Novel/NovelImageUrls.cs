namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Novel image links
/// </summary>
/// <param name="Mini">128x128</param>
/// <param name="Thumb">240x240</param>
/// <param name="Small">480x480</param>
/// <param name="Middle">1200x1200</param>
/// <param name="Original">Original image</param>
public record NovelImageUrls(
    [property: JsonPropertyName("128x128")]
    string Mini,

    [property: JsonPropertyName("240mw")]
    string Thumb,

    [property: JsonPropertyName("480mw")]
    string Small,

    [property: JsonPropertyName("1200x1200")]
    string Middle,

    [property: JsonPropertyName("original")]
    string Original
);
