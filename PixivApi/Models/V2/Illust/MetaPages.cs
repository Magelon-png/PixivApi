namespace Scighost.PixivApi.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="ImageUrls"></param>
public record MetaPages(
    [property: JsonPropertyName("image_urls")]
    ImageUrls ImageUrls
);