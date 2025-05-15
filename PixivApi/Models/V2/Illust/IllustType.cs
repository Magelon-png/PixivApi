using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi;

public partial class PixivClientV2
{
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
}