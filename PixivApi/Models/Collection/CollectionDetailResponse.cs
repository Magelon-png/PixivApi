using System.Text.Json.Nodes;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="TagTranslation"></param>
/// <param name="Thumbnails"></param>
/// <param name="IllustSeries"></param>
/// <param name="Requests"></param>
/// <param name="Users"></param>
/// <param name="Data"></param>
/// <param name="ExtraData"></param>
public record CollectionDetailResponse(
    [property: JsonPropertyName("tagTranslation")]
    Dictionary<string, CollectionTagTranslation> TagTranslation,

    [property: JsonPropertyName("thumbnails")]
    CollectionThumbnails Thumbnails,

    [property: JsonPropertyName("illustSeries")]
    List<JsonNode> IllustSeries,

    [property: JsonPropertyName("requests")]
    List<JsonNode> Requests,

    [property: JsonPropertyName("users")]
    List<CollectionUser> Users,

    [property: JsonPropertyName("data")]
    CollectionDetailData Data,

    [property: JsonPropertyName("extraData")]
    CollectionExtraData ExtraData
);
