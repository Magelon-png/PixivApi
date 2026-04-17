using System.Text.Json.Serialization;

namespace Scighost.PixivApi.Models.V2.User;

public record UserFollowDetailResponse(
    [property: JsonPropertyName("follow_detail")]
    FollowDetail FollowDetail);

public record FollowDetail(
    [property: JsonPropertyName("is_followed")]
    bool IsFollowed,
    [property: JsonPropertyName("restrict")]
    string Restrict);
