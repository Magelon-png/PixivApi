namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Extension"></param>
/// <param name="Width"></param>
/// <param name="Height"></param>
/// <param name="OriginalUrl"></param>
/// <param name="ThumbnailUrl"></param>
public record PostImage(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("extension")]
    string Extension,
    [property: JsonPropertyName("width")]
    int Width,
    [property: JsonPropertyName("height")]
    int Height,
    [property: JsonPropertyName("originalUrl")]
    string OriginalUrl,
    [property: JsonPropertyName("thumbnailUrl")]
    string ThumbnailUrl
);