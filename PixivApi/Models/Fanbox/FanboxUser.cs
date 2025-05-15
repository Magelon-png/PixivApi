namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="IconUrl"></param>
public record FanboxUser([property: JsonPropertyName("userId"), 
JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("iconUrl")]
    string IconUrl);