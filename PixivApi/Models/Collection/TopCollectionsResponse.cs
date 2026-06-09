using System.Numerics;
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
/// <param name="Page"></param>
/// <param name="ZoneConfig"></param>
public record TopCollectionsResponse(
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

    [property: JsonPropertyName("page")]
    TopCollectionsPage Page,

    [property: JsonPropertyName("zoneConfig")]
    CollectionZoneConfig ZoneConfig
);

/// <summary>
/// 
/// </summary>
/// <param name="MyCollectionIds"></param>
/// <param name="RecommendCollectionIds"></param>
/// <param name="EveryoneCollectionIds"></param>
/// <param name="TagRecommendCollectionIds"></param>
/// <param name="CollectionCreationPermission"></param>
public record TopCollectionsPage(
    [property: JsonPropertyName("myCollectionIds")]
    [property: JsonConverter(typeof(BigIntegerListConverter))]
    List<BigInteger> MyCollectionIds,

    [property: JsonPropertyName("recommendCollectionIds")]
    [property: JsonConverter(typeof(BigIntegerListConverter))]
    List<BigInteger> RecommendCollectionIds,

    [property: JsonPropertyName("everyoneCollectionIds")]
    [property: JsonConverter(typeof(BigIntegerListConverter))]
    List<BigInteger> EveryoneCollectionIds,

    [property: JsonPropertyName("tagRecommendCollectionIds")]
    List<TagRecommendCollectionIds> TagRecommendCollectionIds,

    [property: JsonPropertyName("collectionCreationPermission")]
    string CollectionCreationPermission
);

/// <summary>
/// 
/// </summary>
/// <param name="Tag"></param>
/// <param name="Ids"></param>
public record TagRecommendCollectionIds(
    [property: JsonPropertyName("tag")]
    string Tag,

    [property: JsonPropertyName("ids")]
    [property: JsonConverter(typeof(BigIntegerListConverter))]
    List<BigInteger> Ids
);

/// <summary>
/// 
/// </summary>
/// <param name="Header"></param>
/// <param name="Footer"></param>
/// <param name="TopbrandingRectangle"></param>
/// <param name="Comic"></param>
/// <param name="MangatopAppeal"></param>
/// <param name="Logo"></param>
/// <param name="AdLogo"></param>
public record CollectionZoneConfig(
    [property: JsonPropertyName("header")]
    CollectionZoneUrl Header,

    [property: JsonPropertyName("footer")]
    CollectionZoneUrl Footer,

    [property: JsonPropertyName("topbranding_rectangle")]
    CollectionZoneUrl TopbrandingRectangle,

    [property: JsonPropertyName("comic")]
    CollectionZoneUrl Comic,

    [property: JsonPropertyName("mangatop_appeal")]
    CollectionZoneUrl MangatopAppeal,

    [property: JsonPropertyName("logo")]
    CollectionZoneUrl Logo,

    [property: JsonPropertyName("ad_logo")]
    CollectionZoneUrl AdLogo
);

/// <summary>
/// 
/// </summary>
/// <param name="Url"></param>
public record CollectionZoneUrl(
    [property: JsonPropertyName("url")]
    string Url
);
