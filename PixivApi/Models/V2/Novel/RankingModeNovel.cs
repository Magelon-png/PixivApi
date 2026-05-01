using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Novel;

/// <summary>
/// Ranking mode for novels.
/// </summary>
[EnumExtensions]
public enum RankingModeNovel
{
    /// <summary>
    /// Daily ranking.
    /// </summary>
    [Display(Name = "day")]
    Day,

    /// <summary>
    /// Daily ranking for male users.
    /// </summary>
    [Display(Name = "day_male")]
    DayMale,

    /// <summary>
    /// Daily ranking for female users.
    /// </summary>
    [Display(Name = "day_female")]
    DayFemale,

    /// <summary>
    /// Weekly ranking for rookie works.
    /// </summary>
    [Display(Name = "week_rookie")]
    WeekRookie,

    /// <summary>
    /// Weekly ranking.
    /// </summary>
    [Display(Name = "week")]
    Week,
    
    /// <summary>
    /// Daily R18 ranking.
    /// </summary>
    [Display(Name = "day_r18")]
    DayR18,

    /// <summary>
    /// Weekly R18 ranking.
    /// </summary>
    [Display(Name = "week_r18")]
    WeekR18
}
