using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum IllustrationContentType
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
    [Display(Name = "novel")]
    Novel,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "user")]
    User
}