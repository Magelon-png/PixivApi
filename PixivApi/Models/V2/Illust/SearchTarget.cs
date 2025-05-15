using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;


    [EnumExtensions]
    public enum SearchTarget
    {
        [Display(Name = "partial_match_for_tags")]
        PartialMatchForTags,
        [Display(Name = "exact_match_for_tags")]
        ExactMatchForTags,
        [Display(Name = "title_and_caption")]
        TitleAndCaption,
        [Display(Name = "text")]
        Text,
        [Display(Name = "keyword")]
        Keyword
    }