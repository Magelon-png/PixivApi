using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum IllustAiType
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name="Not AI")]
    NotAi = 0 | 1,
    /// <summary>
    /// 
    /// </summary>
    [Display]
    Ai = 2
}