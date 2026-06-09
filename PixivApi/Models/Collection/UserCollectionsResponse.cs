using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="TagTranslation"></param>
/// <param name="Thumbnails"></param>
/// <param name="IllustSeries"></param>
/// <param name="Requests"></param>
/// <param name="Users"></param>
/// <param name="Works"></param>
/// <param name="ExtraData"></param>
public record UserCollectionsResponse(
    [property: JsonPropertyName("tagTranslation")]
    [property: JsonConverter(typeof(EmptyArrayAsEmptyDictionaryConverterCollectionTagTranslation))]
    Dictionary<string, CollectionTagTranslation> TagTranslation,

    [property: JsonPropertyName("thumbnails")]
    CollectionThumbnails Thumbnails,

    [property: JsonPropertyName("illustSeries")]
    List<JsonNode> IllustSeries,

    [property: JsonPropertyName("requests")]
    List<JsonNode> Requests,

    [property: JsonPropertyName("users")]
    List<CollectionUser> Users,

    [property: JsonPropertyName("works")]
    List<CollectionProfile> Works,

    [property: JsonPropertyName("extraData")]
    UserCollectionsExtraData ExtraData
);

/// <summary>
/// 
/// </summary>
/// <param name="Meta"></param>
public record UserCollectionsExtraData(
    [property: JsonPropertyName("meta")]
    CollectionMetaWithAlternateLanguages Meta
);
