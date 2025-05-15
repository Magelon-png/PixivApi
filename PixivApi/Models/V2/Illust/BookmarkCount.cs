using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

    [EnumExtensions]
    public enum BookmarkCount
    {
        [Display(Name = "0")]
        None = 0,
        [Display(Name = "10")]
        Ten = 1,
        [Display(Name = "30")]
        Thirty = 2,
        [Display(Name = "50")]
        Fifty = 3,
        [Display(Name = "100")]
        Hundred = 4,
        [Display(Name = "300")]
        ThreeHundred = 5,
        [Display(Name = "500")]
        FiveHundred = 6,
        [Display(Name = "1000")]
        Thousand = 7,
        [Display(Name = "5000")]
        FiveThousand = 8
}