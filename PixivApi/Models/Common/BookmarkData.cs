namespace Scighost.PixivApi.Models.Common;

/// <summary>
/// Bookmark attributes of a work
/// </summary>
/// <param name="Id"></param>
/// <param name="Private"></param>
public record BookmarkData(
    [property: JsonPropertyName("id"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    long Id,
    [property: JsonPropertyName("private")]
    bool Private
);