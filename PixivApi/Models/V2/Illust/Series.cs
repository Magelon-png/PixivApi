namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
public record Series(
    [property: JsonPropertyName("id"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    uint Id,
    [property: JsonPropertyName("title")]
    string Title
);