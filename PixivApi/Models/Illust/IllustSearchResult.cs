namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Illust"></param>
/// <param name="Popular"></param>
/// <param name="RelatedTags"></param>
/// <param name="TagTranslation"></param>
/// <param name="ExtraData"></param>
public record IllustSearchResult(
    [property: JsonPropertyName("illust")] IllustSearchInfo Illust,
    [property: JsonPropertyName("popular")] IllustSearchPopularInfo Popular,
    [property: JsonPropertyName("relatedTags")] string[] RelatedTags,
    [property: JsonPropertyName("tagTranslation")] Dictionary<string,IllustSearchTagTranslation> TagTranslation,
    [property: JsonPropertyName("extraData")] IllustSearchExtraData ExtraData
);