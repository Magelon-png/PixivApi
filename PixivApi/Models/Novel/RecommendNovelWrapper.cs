namespace Scighost.PixivApi.Models.Novel;

internal record RecommendNovelWrapper(
    [property: JsonPropertyName("novels")]
    List<NovelProfile> Novels,

    [property: JsonPropertyName("nextIds")]
    List<string> NextIds
);
