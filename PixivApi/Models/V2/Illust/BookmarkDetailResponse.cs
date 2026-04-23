using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="BookmarkDetail"></param>
public record BookmarkDetailResponse(
    [property: JsonPropertyName("bookmark_detail")] 
    BookmarkDetail BookmarkDetail);

/// <summary>
/// 
/// </summary>
/// <param name="IsBookmarked"></param>
/// <param name="Restrict"></param>
/// <param name="Tags"></param>
public record BookmarkDetail(
    [property: JsonPropertyName("is_bookmarked")]
    bool IsBookmarked,
    [property: JsonPropertyName("restrict")]
    BookmarkRestrictionScope Restrict,
    [property: JsonPropertyName("tags")]
    List<BookmarkTagV2> Tags
);

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum BookmarkRestrictionScope
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "all")]
    All,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "public")]
    Public,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "private")]
    Private
}

/// <summary>
/// 
/// </summary>
/// <param name="Count"></param>
/// <param name="IsRegistered"></param>
/// <param name="Name"></param>
public record BookmarkTagV2(
    [property: JsonPropertyName("count")]
    int Count,
    [property: JsonPropertyName("is_registered")]
    bool IsRegistered,
    [property: JsonPropertyName("name")]
    string Name
);