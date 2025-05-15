using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

[EnumExtensions]
public enum IllustAiType
{
    [Display(Name="Not AI")]
    NotAi = 0 | 1,
    [Display]
    Ai = 2
}