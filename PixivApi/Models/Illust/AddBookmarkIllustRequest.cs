using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;


/// <summary>
/// Add illustration bookmark request
/// </summary>
/// <param name="IllustId">Illustration id</param>
/// <param name="IsPrivate">Private</param>
/// <param name="Comment">Comment</param>
/// <param name="Tags">Custom tags</param>
public record AddBookmarkIllustRequest(
    [property: JsonPropertyName("illust_id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int IllustId,

    [property: JsonPropertyName("restrict")]
    [property: JsonConverter(typeof(BoolToNumberJsonConverter))]
    bool IsPrivate,

    [property: JsonPropertyName("comment")]
    string Comment,

    [property: JsonPropertyName("tags")]
    IEnumerable<string> Tags
);
