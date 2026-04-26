namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// Result of recommended or followed Pixiv creators search
/// </summary>
/// <param name="Creators">List of creators</param>
public record SearchRecommendCreatorsResult(
    [property: JsonPropertyName("creators")] FollowedCreator[] Creators);