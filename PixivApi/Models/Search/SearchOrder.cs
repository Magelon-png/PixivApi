using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Search;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum SearchOrder
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name= "date_d")]
    DateDescending,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "date")]
    DateAscending,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "popular_d")]
    PopularityDescending,
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "popular")]
    PopularityAscending,
}