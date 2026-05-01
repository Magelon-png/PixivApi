namespace Scighost.PixivApi.Models.Novel;

internal sealed record BookmarkNovelWrapper(
    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("works")]
    List<NovelProfile> Works
);

internal sealed record BookmarkNovelWrapperV2(
    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("works")]
    List<NovelProfileBookmarks> Works
);