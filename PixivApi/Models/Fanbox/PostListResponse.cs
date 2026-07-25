namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// Wrapper for the post list response
/// </summary>
/// <param name="Posts">Array of post list items</param>
public record PostListResponse(
    [property: JsonPropertyName("posts")]
    PostListItem[] Posts
);
