using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.Search;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum SearchAge
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "all")]
    AnyAge,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "safe")]
    AllAges,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "r18")]
    R18
}