using System.Text.Json.Serialization;

namespace Scighost.PixivApi.Models.V2.User;

/// <summary>
/// 
/// </summary>
/// <param name="FollowDetail"></param>
public record UserFollowDetailResponse(
    [property: JsonPropertyName("follow_detail")]
    FollowDetail FollowDetail);

/// <summary>
/// 
/// </summary>
/// <param name="IsFollowed"></param>
/// <param name="Restrict"></param>
public record FollowDetail(
    [property: JsonPropertyName("is_followed")]
    bool IsFollowed,
    [property: JsonPropertyName("restrict")]
    string Restrict);
