using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

public record BookmarkDetailResponse(
    [property: JsonPropertyName("bookmark_detail")] 
    BookmarkDetail BookmarkDetail);

public record BookmarkDetail(
    [property: JsonPropertyName("is_bookmarked")]
    bool IsBookmarked,
    [property: JsonPropertyName("restrict")]
    BookmarkRestrictionScope Restrict,
    [property: JsonPropertyName("tags")]
    List<BookmarkTag> Tags
);

[EnumExtensions]
public enum BookmarkRestrictionScope
{
    [Display(Name = "all")]
    All,
    [Display(Name = "public")]
    Public,
    [Display(Name = "private")]
    Private
}

public record BookmarkTag(
    [property: JsonPropertyName("count")]
    int Count,
    [property: JsonPropertyName("is_registered")]
    bool IsRegistered,
    [property: JsonPropertyName("name")]
    string Name
);