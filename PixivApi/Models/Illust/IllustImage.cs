namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 不同尺寸的图片文件地址
/// </summary>
public record IllustImage(
    [property: JsonPropertyName("urls")]
    IllustImageUrls Urls,
    
    [property: JsonPropertyName("width")]
    int Width,
    
    [property: JsonPropertyName("height")]
    int Height
);