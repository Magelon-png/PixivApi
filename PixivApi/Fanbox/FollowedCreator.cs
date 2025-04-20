namespace Scighost.PixivApi.Fanbox;


/// <summary>
/// 
/// </summary>
/// <param name="User"></param>
/// <param name="CreatorId"></param>
/// <param name="Description"></param>
/// <param name="HasAdultContent"></param>
/// <param name="CoverImageUrl"></param>
/// <param name="ProfileLinks"></param>
/// <param name="ProfileItems"></param>
/// <param name="IsFollowed"></param>
/// <param name="IsSupported"></param>
/// <param name="IsStopped"></param>
/// <param name="IsAcceptingRequest"></param>
/// <param name="HasBoothShop"></param>
/// <param name="HasPublishedPost"></param>
/// <param name="Category"></param>
public record FollowedCreator(
    [property: JsonPropertyName("user")]
    FanboxUser User,
    [property: JsonPropertyName("creatorId")]
    string CreatorId,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("hasAdultContent")]
    bool HasAdultContent,
    [property: JsonPropertyName("coverImageUrl")]
    string CoverImageUrl,
    [property: JsonPropertyName("profileLinks")]
    string[] ProfileLinks,
    [property: JsonPropertyName("profileItems")]
    UserProfileItem[] ProfileItems,
    [property: JsonPropertyName("isFollowed")]
    bool IsFollowed,
    [property: JsonPropertyName("isSupported")]
    bool IsSupported,
    [property: JsonPropertyName("isStopped")]
    bool IsStopped,
    [property: JsonPropertyName("isAcceptingRequest")]
    bool IsAcceptingRequest,
    [property: JsonPropertyName("hasBoothShop")]
    bool HasBoothShop,
    [property: JsonPropertyName("hasPublishedPost")]
    bool HasPublishedPost,
    [property: JsonPropertyName("category")]
    string Category
);