using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// Ranking mode for illustrations.
/// </summary>
[EnumExtensions]
public enum RankingMode
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
    /// Weekly ranking for original works.
    /// </summary>
    [Display(Name = "week_original")]
    WeekOriginal,

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
    /// Monthly ranking.
    /// </summary>
    [Display(Name = "month")]
    Month,

    /// <summary>
    /// Daily R18 ranking.
    /// </summary>
    [Display(Name = "day_r18")]
    DayR18,

    /// <summary>
    /// Weekly R18 ranking.
    /// </summary>
    [Display(Name = "week_r18")]
    WeekR18,

    /// <summary>
    /// Daily R18 ranking for male users.
    /// </summary>
    [Display(Name = "day_male_r18")]
    DayMaleR18,

    /// <summary>
    /// Daily R18 ranking for female users.
    /// </summary>
    [Display(Name = "day_female_r18")]
    DayFemaleR18,

    /// <summary>
    /// Weekly R18G ranking.
    /// </summary>
    [Display(Name = "week_r18g")]
    WeekR18G,

    /// <summary>
    /// Weekly ranking for rookie manga.
    /// </summary>
    [Display(Name = "week_rookie_manga")]
    WeekRookieManga,

    /// <summary>
    /// Weekly manga ranking.
    /// </summary>
    [Display(Name = "week_manga")]
    WeekManga,

    /// <summary>
    /// Monthly manga ranking.
    /// </summary>
    [Display(Name = "month_manga")]
    MonthManga,

    /// <summary>
    /// Daily R18 manga ranking.
    /// </summary>
    [Display(Name = "day_r18_manga")]
    DayR18Manga,

    /// <summary>
    /// Weekly R18 manga ranking.
    /// </summary>
    [Display(Name = "week_r18_manga")]
    WeekR18Manga,

    /// <summary>
    /// Weekly R18G manga ranking.
    /// </summary>
    [Display(Name = "week_r18g_manga")]
    WeekR18GManga
}