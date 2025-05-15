using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;


    [EnumExtensions]
    public enum SearchPeriod
    {
        [Display(Name = "within_last_day")]
        LastDay,
        [Display(Name = "within_last_week")]
        LastWeek,
        [Display(Name = "within_last_month")]
        LastMonth
    }