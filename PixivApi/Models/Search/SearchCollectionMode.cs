using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.Search;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum SearchCollectionMode
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "safe")]
    Safe,

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "r18")]
    R18
}
