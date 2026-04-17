using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;


    /// <summary>
    /// 
    /// </summary>
    [EnumExtensions]
    public enum IllustType
    {
        [Display(Name = "illust")]
        Illust,
        [Display(Name = "manga")]
        Manga,
        [Display(Name = "illust_manga")]
        IllustManga,
        [Display(Name = "novel")]
        Novel
    }