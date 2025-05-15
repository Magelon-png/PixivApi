namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Type"></param>
/// <param name="Url"></param>
public record PostCover(
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("url")]
    string Url
);