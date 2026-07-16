namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// Wrapper for the followed creators response
/// </summary>
/// <param name="Creators">Array of followed creators</param>
public record FollowedCreatorsResponse(
    [property: JsonPropertyName("creators")]
    FollowedCreator[] Creators
);
