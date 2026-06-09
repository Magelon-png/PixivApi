using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="RecommendedTags"></param>
/// <param name="TagTranslation"></param>
public record RecommendedCollectionTags(
    [property: JsonPropertyName("recommendedTags")]
    List<string> RecommendedTags,

    [property: JsonPropertyName("tagTranslation")]
    [property: JsonConverter(typeof(EmptyArrayAsEmptyDictionaryConverterCollectionTagTranslation))]
    Dictionary<string, CollectionTagTranslation> TagTranslation
);
