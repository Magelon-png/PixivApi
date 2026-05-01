using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// Search order for illustrations.
/// </summary>
[EnumExtensions]
public enum SearchOrderV2
{
    /// <summary>
    /// Sort by date in descending order (newest first).
    /// </summary>
    [Display(Name = "date_desc")]
    DateDescending,

    /// <summary>
    /// Sort by date in ascending order (oldest first).
    /// </summary>
    [Display(Name = "date_asc")]
    DateAscending,

    /// <summary>
    /// Sort by popularity in descending order (most popular first).
    /// </summary>
    [Display(Name = "popular_desc")]
    PopularDescending
}
