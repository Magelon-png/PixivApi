namespace Scighost.PixivApi.Models.User;

/// <summary>
/// Simple user information
/// </summary>
/// <param name="UserId">User uid</param>
/// <param name="Name">Nickname</param>
/// <param name="Image">Avatar small image</param>
/// <param name="ImageBig">Avatar large image</param>
/// <param name="Premium">Premium member</param>
/// <param name="IsFollowed">Followed</param>
/// <param name="IsMypixiv">My Pixiv friend</param>
/// <param name="IsBlocking">Blocked</param>
/// <param name="Comment">Personal introduction</param>
/// <param name="FollowedBack">Followed back</param>
/// <param name="AcceptRequest">Accept commissions</param>
public record UserProfile(
    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("image")]
    string Image,

    [property: JsonPropertyName("imageBig")]
    string ImageBig,

    [property: JsonPropertyName("premium")]
    bool Premium,

    [property: JsonPropertyName("isFollowed")]
    bool IsFollowed,

    [property: JsonPropertyName("isMypixiv")]
    bool IsMypixiv,

    [property: JsonPropertyName("isBlocking")]
    bool IsBlocking,

    [property: JsonPropertyName("comment")]
    string Comment,

    [property: JsonPropertyName("followedBack")]
    bool FollowedBack,

    [property: JsonPropertyName("acceptRequest")]
    bool AcceptRequest
);