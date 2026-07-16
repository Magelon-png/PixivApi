using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// Response containing the list of bookmarked illustrations
/// </summary>
/// <param name="Works">List of illustration profiles</param>
/// <param name="Total">Total number of bookmarked illustrations</param>
/// <param name="BookmarkTags">Dictionary mapping illust ID to its custom tags</param>
public record GetUserBookmarkIllustsResponse(
    [property: JsonPropertyName("works")]
    List<IllustProfile> Works,
    [property: JsonPropertyName("total")]
    int Total,
    [property: JsonPropertyName("bookmarkTags")]
    [property: JsonConverter(typeof(BookmarkTagsConverter))]
    Dictionary<long, string[]> BookmarkTags
    );