namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Image embedded in the novel content
/// </summary>
/// <param name="NovelImageId">Image ID</param>
/// <param name="Urls">Image links</param>
public record TextEmbeddedImage(
    [property: JsonPropertyName("novelImageId")]
    int NovelImageId,

    [property: JsonPropertyName("urls")]
    NovelImageUrls Urls
);