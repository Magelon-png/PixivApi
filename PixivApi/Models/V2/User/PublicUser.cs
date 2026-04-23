namespace Scighost.PixivApi.Models.V2.User;

/// <summary>
/// Represents a public user profile information.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="ProfileImageUrls">The profile image URLs for the user.</param>
/// <param name="Name">The display name of the user.</param>
/// <param name="Account">Will resolve to the user ID since it is not public information.</param>
/// <param name="IsFollowed">Whether the authenticated user is following this user.</param>
/// <param name="IsAccessBlockingUser">Whether the user is blocking access.</param>
/// <param name="AcceptRequests">Whether the user accepts requests.</param>
/// <param name="Comment">Not null when retrieved from comments.</param>
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
    bool AcceptRequests,
    [property: JsonPropertyName("comment")]
    string? Comment
);