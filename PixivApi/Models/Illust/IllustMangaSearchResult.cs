namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// Response body of the artworks search endpoint, which returns its results under
/// <c>illustManga</c> instead of <c>illust</c>.
/// </summary>
internal sealed record IllustMangaSearchResult(
    [property: JsonPropertyName("illustManga")] IllustSearchInfo IllustManga,
    [property: JsonPropertyName("popular")] IllustSearchPopularInfo Popular,
    [property: JsonPropertyName("relatedTags")] string[] RelatedTags,
    [property: JsonPropertyName("tagTranslation")] Dictionary<string, IllustSearchTagTranslation> TagTranslation,
    [property: JsonPropertyName("extraData")] IllustSearchExtraData ExtraData
);
