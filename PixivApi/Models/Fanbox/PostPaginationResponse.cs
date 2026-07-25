namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// Wrapper for the post pagination response
/// </summary>
/// <param name="PageUrls">Array of paginated post listing URLs</param>
public record PostPaginationResponse(
    [property: JsonPropertyName("pageUrls")]
    string[] PageUrls
);
