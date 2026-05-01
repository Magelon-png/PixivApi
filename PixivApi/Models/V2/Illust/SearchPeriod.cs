using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// Search period filter for illustrations.
/// </summary>
[EnumExtensions]
public enum SearchPeriod
{
    /// <summary>
    /// Search within the last day.
    /// </summary>
    [Display(Name = "within_last_day")]
    LastDay,

    /// <summary>
    /// Search within the last week.
    /// </summary>
    [Display(Name = "within_last_week")]
    LastWeek,

    /// <summary>
    /// Search within the last month.
    /// </summary>
    [Display(Name = "within_last_month")]
    LastMonth
}
