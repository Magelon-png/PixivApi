namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="En"></param>
/// <param name="Ko"></param>
/// <param name="Zh"></param>
/// <param name="ZhTw"></param>
/// <param name="Romaji"></param>
public record CollectionTagTranslation(
    [property: JsonPropertyName("en")]
    string En,

    [property: JsonPropertyName("ko")]
    string Ko,

    [property: JsonPropertyName("zh")]
    string Zh,

    [property: JsonPropertyName("zh_tw")]
    string ZhTw,

    [property: JsonPropertyName("romaji")]
    string Romaji
);
