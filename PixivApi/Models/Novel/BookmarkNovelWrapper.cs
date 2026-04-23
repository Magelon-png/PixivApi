namespace Scighost.PixivApi.Models.Novel;

internal sealed record BookmarkNovelWrapper(
    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("works")]
    List<NovelProfile> Works
);