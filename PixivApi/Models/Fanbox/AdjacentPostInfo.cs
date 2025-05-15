namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="PublishedDatetime"></param>
public record AdjacentPostInfo(
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString),
               JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("title")]
    string Title,
    [property: JsonPropertyName("publishedDatetime")]
    DateTimeOffset PublishedDatetime
);