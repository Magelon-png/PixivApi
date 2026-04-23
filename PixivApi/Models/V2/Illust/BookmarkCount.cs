using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

    /// <summary>
    /// 
    /// </summary>
    [EnumExtensions]
    public enum BookmarkCount
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "0")]
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "10")]
        Ten = 1,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "30")]
        Thirty = 2,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "50")]
        Fifty = 3,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "100")]
        Hundred = 4,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "300")]
        ThreeHundred = 5,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "500")]
        FiveHundred = 6,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "1000")]
        Thousand = 7,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "5000")]
        FiveThousand = 8
}