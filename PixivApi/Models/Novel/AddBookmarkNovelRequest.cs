using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;


/// <summary>
/// Request to add a novel bookmark
/// </summary>
/// <param name="NovelId">Novel id</param>
/// <param name="IsPrivate">Private</param>
/// <param name="Comment">Comment</param>
/// <param name="Tags">Custom tags</param>
public record AddBookmarkNovelRequest(
    [property: JsonPropertyName("novel_id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int NovelId,

    [property: JsonPropertyName("restrict")]
    [property: JsonConverter(typeof(BoolToNumberJsonConverter))]
    bool IsPrivate,

    [property: JsonPropertyName("comment")]
    string Comment,

    [property: JsonPropertyName("tags")]
    IEnumerable<string> Tags
);