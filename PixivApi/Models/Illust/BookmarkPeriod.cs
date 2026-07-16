using System.ComponentModel;
using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// A bookmark period entry
/// </summary>
/// <param name="Period">The date and month where a bookmark has been created</param>
/// <param name="Count">The total amount of bookmarks created in the period</param>
public record BookmarkPeriod(
    [property: JsonPropertyName("period")]
    [property: JsonConverter(typeof(BookmarkPeriodDateConverter))]
    DateOnly Period,
    [property: JsonPropertyName("count")]
    int Count
    );