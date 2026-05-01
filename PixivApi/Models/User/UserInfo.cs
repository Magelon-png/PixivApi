using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.User;


/// <summary>
/// User information
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
/// <param name="Background">User personal background, this value is null as the API can no longer retrieve the background image</param>
/// <param name="FollowingCount">Number of users followed by this user</param>
/// <param name="CommentHtml">Personal introduction, HTML format</param>
/// <param name="Webpage">Webpage</param>
/// <param name="Official">Official</param>
/// <param name="Group">Group</param>
/// <param name="Partial">If the provided profile is containing partial information</param>
public record UserInfo(
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
    bool AcceptRequest,

    [property: JsonPropertyName("background")]
    UserBackground? Background,

    [property: JsonPropertyName("following")]
    int FollowingCount,

    [property: JsonPropertyName("commentHtml")]
    string CommentHtml,

    [property: JsonPropertyName("webpage")]
    string Webpage,

    [property: JsonPropertyName("official")]
    bool Official,

    [property: JsonPropertyName("group")]
    object Group,

    [property: JsonPropertyName("partial")]
    [property: JsonConverter(typeof(BoolToNumberJsonConverter))]
    bool Partial
) : UserProfile(UserId, Name, Image, ImageBig, Premium, IsFollowed, IsMypixiv, IsBlocking, Comment, FollowedBack, AcceptRequest);


/// <summary>
/// User personal background image
/// </summary>
/// <param name="Repeat">Repeat</param>
/// <param name="Color">Color</param>
/// <param name="Url">Image URL, may be missing</param>
/// <param name="IsPrivate">Is private</param>
public record UserBackground(
    [property: JsonPropertyName("repeat")]
    object Repeat,

    [property: JsonPropertyName("color")]
    object Color,

    [property: JsonPropertyName("url")]
    string Url,

    [property: JsonPropertyName("isPrivate")]
    bool IsPrivate
);