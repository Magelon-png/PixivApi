using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Search;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum SearchLanguage
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "en")]
    English
}