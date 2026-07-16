namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// Wrapper for the post info response
/// </summary>
/// <param name="Post">Post information</param>
public record PostInfoResponse(
    [property: JsonPropertyName("post")]
    PostInfo Post
);
