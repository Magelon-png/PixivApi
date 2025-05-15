using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi;

public partial class PixivClientV2
{
    [EnumExtensions]
    public enum SearchOrderV2
    {
        [Display(Name = "date_desc")]
        DateDescending,
        [Display(Name = "date_asc")]
        DateAscending,
        [Display(Name = "popular_desc")]
        PopularDescending
    }
}