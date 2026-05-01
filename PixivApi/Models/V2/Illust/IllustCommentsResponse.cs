using Scighost.PixivApi.Models.V2.User;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Comments"></param>
public record IllustCommentsResponse(
    [property: JsonPropertyName("comments")]
    List<IllustComment> Comments
    );

/// <summary>
/// 
/// </summary>
/// <param name="Comment"></param>
/// <param name="Date"></param>
/// <param name="Id"></param>
/// <param name="User"></param>
public record IllustComment(
[property: JsonPropertyName("comment")]
    string Comment,
    [property: JsonPropertyName("date")]
    DateTimeOffset Date,
    [property: JsonPropertyName("id"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,
    [property: JsonPropertyName("user")]
    PublicUser User
    );