namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Data"></param>
/// <param name="Total"></param>
/// <param name="LastPage"></param>
/// <param name="BookmarkRanges"></param>
public record IllustSearchInfo(
    [property: JsonPropertyName("data")] IllustSearchData[] Data,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("lastPage")] int LastPage,
    [property: JsonPropertyName("bookmarkRanges")] IllustSearchBookmarkRanges[] BookmarkRanges
);