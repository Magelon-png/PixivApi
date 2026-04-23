using System.Text.Json.Serialization;
using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.V2.User;

/// <summary>
/// 
/// </summary>
/// <param name="BookmarkTags"></param>
/// <param name="NextUrl"></param>
public record UserBookmarkTagsResponse(
    [property: JsonPropertyName("bookmark_tags")]
    List<BookmarkTag> BookmarkTags,
    [property: JsonPropertyName("next_url")]
    string? NextUrl);
