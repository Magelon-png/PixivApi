namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Items"></param>
/// <param name="NextUrl"></param>
public record HomePagePostItems(
    [property: JsonPropertyName("items")]
    PostListItem[] Items,
    [property: JsonPropertyName("nextUrl")]
    string NextUrl
    );