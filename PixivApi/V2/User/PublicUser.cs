namespace Scighost.PixivApi.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="ProfileImageUrls"></param>
/// <param name="Name"></param>
/// <param name="Account">Will resolve to the user ID since it is not public information</param>
/// <param name="IsFollowed"></param>
public record PublicUser(
    [property: JsonPropertyName("id"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    uint Id,
    [property: JsonPropertyName("profile_image_urls")]
    ProfileImageUrls ProfileImageUrls,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("account")]
    string Account,
    [property: JsonPropertyName("is_followed")]
    bool IsFollowed,
    [property: JsonPropertyName("is_access_blocking_user")]
    bool IsAccessBlockingUser,
    [property: JsonPropertyName("is_accept_request")]
    bool AcceptRequests
);