using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Search;

/// <summary>
/// Change favorite tags request
/// </summary>
/// <param name="Tags">Tags</param>
public record ChangeFavoriteTagsRequest(
    [property: JsonPropertyName("tags")]
    IEnumerable<string> Tags
);
