namespace Scighost.PixivApi.Models.Illust;

internal record BookmarkIllustWrapper(
    [property: JsonPropertyName("total")]
    int Total,

    [property: JsonPropertyName("works")]
    List<IllustProfile> Works
);
