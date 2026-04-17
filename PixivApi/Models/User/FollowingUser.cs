using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.User;

/// <summary>
/// Followed user
/// </summary>
/// <param name="UserId">User uid</param>
/// <param name="UserName">Nickname</param>
/// <param name="UserProfileImageUrl">Avatar image</param>
/// <param name="Illusts">Illustration works</param>
/// <param name="Novels">Novel works</param>
public record FollowingUser(
    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("profileImageUrl")]
    string UserProfileImageUrl,

    [property: JsonPropertyName("illusts")]
    List<IllustProfile> Illusts,

    [property: JsonPropertyName("novels")]
    List<NovelProfile> Novels
);

internal record FollowingUserWrapper(
    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("users")]
    List<FollowingUser> Users
);