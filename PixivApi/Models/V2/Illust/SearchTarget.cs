using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// Search target for filtering search results.
/// </summary>
[EnumExtensions]
public enum SearchTarget
{
    /// <summary>
    /// Partial match for tags.
    /// </summary>
    [Display(Name = "partial_match_for_tags")]
    PartialMatchForTags,

    /// <summary>
    /// Exact match for tags.
    /// </summary>
    [Display(Name = "exact_match_for_tags")]
    ExactMatchForTags,

    /// <summary>
    /// Search in title and caption.
    /// </summary>
    [Display(Name = "title_and_caption")]
    TitleAndCaption,

    /// <summary>
    /// Search in text content.
    /// </summary>
    [Display(Name = "text")]
    Text,

    /// <summary>
    /// Search by keyword.
    /// </summary>
    [Display(Name = "keyword")]
    Keyword
}
