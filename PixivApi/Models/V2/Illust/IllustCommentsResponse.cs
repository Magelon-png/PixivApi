using Scighost.PixivApi.Models.V2.User;

namespace Scighost.PixivApi.Models.V2.Illust;

public record IllustCommentsResponse(
    [property: JsonPropertyName("comments")]
    List<IllustComment> Comments
    );

public record IllustComment(
[property: JsonPropertyName("comment")]
    string Comment,
    [property: JsonPropertyName("date")]
    DateTimeOffset Date,
    [property: JsonPropertyName("id"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    uint Id,
    [property: JsonPropertyName("user")]
    PublicUser User
    );