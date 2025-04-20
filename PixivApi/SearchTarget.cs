using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum SearchTarget
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "all")]
    Any,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "illust")]
    Illust,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "illust_and_ugoira")]
    IllustAndUgoira,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "manga")]
    Manga,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "ugoira")]
    Ugoira
}