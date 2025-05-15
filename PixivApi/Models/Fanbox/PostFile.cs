namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Url"></param>
/// <param name="Name"></param>
/// <param name="Size"></param>
/// <param name="Id"></param>
/// <param name="Extension"></param>
public record PostFile(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("size")]
    long Size,
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("extension")]
    string Extension);