using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Novel;

[EnumExtensions]
public enum RankingModeNovel
{
    [Display(Name = "day")]
    Day,

    [Display(Name = "day_male")]
    DayMale,

    [Display(Name = "day_female")]
    DayFemale,

    [Display(Name = "week_rookie")]
    WeekRookie,

    [Display(Name = "week")]
    Week,
    
    [Display(Name = "day_r18")]
    DayR18,

    [Display(Name = "week_r18")]
    WeekR18
}
