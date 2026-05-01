namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// Image file addresses of different sizes
/// </summary>
public record IllustImage(
    [property: JsonPropertyName("urls")]
    IllustImageUrls Urls,
    
    [property: JsonPropertyName("width")]
    int Width,
    
    [property: JsonPropertyName("height")]
    int Height
);