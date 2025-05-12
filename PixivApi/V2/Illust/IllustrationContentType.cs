using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.V2.Illust;

[EnumExtensions]
public enum IllustrationContentType
{
    [Display(Name = "illust")]
    Illust,
    [Display(Name = "manga")]
    Manga,
    [Display(Name = "novel")]
    Novel,
    [Display(Name = "user")]
    User
}