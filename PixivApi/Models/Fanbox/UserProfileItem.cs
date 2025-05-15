namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Type"></param>
/// <param name="ImageUrl"></param>
/// <param name="ThumbnailUrl"></param>
public record UserProfileItem(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("imageUrl")]
    string ImageUrl,
    [property: JsonPropertyName("thumbnailUrl")]
    string ThumbnailUrl
);