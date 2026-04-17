namespace Scighost.PixivApi.Models.Novel;

internal record BookmarkNovelWrapper(
    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("works")]
    List<NovelProfile> Works
);