using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;


    /// <summary>
    /// 
    /// </summary>
    [EnumExtensions]
    public enum IllustType
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "illust")]
        Illust,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "manga")]
        Manga,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "illust_manga")]
        IllustManga,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "novel")]
        Novel
    }