using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.User;

/// <summary>
/// Recommended user
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
/// <param name="Illusts">Illustration works</param>
/// <param name="Novels">Novel works</param>
public record RecommendUser(
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

    [property: JsonIgnore]
    List<IllustProfile> Illusts,

    [property: JsonIgnore]
    List<NovelProfile> Novels
) : UserProfile(UserId, Name, Image, ImageBig, Premium, IsFollowed, IsMypixiv, IsBlocking, Comment, FollowedBack, AcceptRequest);


internal sealed record RecommendUserResponse(
    [property: JsonPropertyName("recommendUsers")]
    List<RecommendMap> RecommendMaps,

    [property: JsonPropertyName("thumbnails")]
    Thumbnails Thumbnails,

    [property: JsonPropertyName("users")]
    List<RecommendUser> Users
);


internal sealed record RecommendMap(
    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("illustIds")]
    List<string> IllustIds,

    [property: JsonPropertyName("novelIds")]
    List<string> NovelIds
);
