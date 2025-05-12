using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.V2.Illust;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum RankingMode
{
    [Display(Name = "day")]
    Day,

    [Display(Name = "day_male")]
    DayMale,

    [Display(Name = "day_female")]
    DayFemale,

    [Display(Name = "week_original")]
    WeekOriginal,

    [Display(Name = "week_rookie")]
    WeekRookie,

    [Display(Name = "week")]
    Week,

    [Display(Name = "month")]
    Month,

    [Display(Name = "day_r18")]
    DayR18,

    [Display(Name = "week_r18")]
    WeekR18,

    [Display(Name = "day_male_r18")]
    DayMaleR18,

    [Display(Name = "day_female_r18")]
    DayFemaleR18,

    [Display(Name = "week_r18g")]
    WeekR18G,

    [Display(Name = "week_rookie_manga")]
    WeekRookieManga,

    [Display(Name = "week_manga")]
    WeekManga,

    [Display(Name = "month_manga")]
    MonthManga,

    [Display(Name = "day_r18_manga")]
    DayR18Manga,

    [Display(Name = "week_r18_manga")]
    WeekR18Manga,

    [Display(Name = "week_r18g_manga")]
    WeekR18GManga
}