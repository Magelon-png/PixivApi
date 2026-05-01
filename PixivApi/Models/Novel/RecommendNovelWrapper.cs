namespace Scighost.PixivApi.Models.Novel;

internal sealed record RecommendNovelWrapper(
    [property: JsonPropertyName("novels")]
    List<NovelProfile> Novels,

    [property: JsonPropertyName("nextIds")]
    List<string> NextIds
);
