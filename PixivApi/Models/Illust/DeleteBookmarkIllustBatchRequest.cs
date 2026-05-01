using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
///
/// </summary>
/// <param name="BookmarkIds"></param>
public record DeleteBookmarkIllustBatchRequest(
    [property: JsonPropertyName("bookmarkIds"),
               JsonConverter(typeof(ListLongAsDictionaryIndexJsonConverter))]
    List<long> BookmarkIds);