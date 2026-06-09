using System.Numerics;
using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="Detail"></param>
/// <param name="Citations"></param>
/// <param name="UserCollections"></param>
public record CollectionDetailData(
    [property: JsonPropertyName("detail")]
    CollectionDetailValue Detail,

    [property: JsonPropertyName("citations")]
    List<JsonNode> Citations,

    [property: JsonPropertyName("userCollections")]
    [property: JsonConverter(typeof(BigIntegerDictionaryConverterCollectionProfile))]
    Dictionary<BigInteger, CollectionProfile> UserCollections
);

/// <summary>
/// 
/// </summary>
/// <param name="Tags"></param>
/// <param name="Tiles"></param>
public record CollectionDetailValue(
    [property: JsonPropertyName("tags")]
    CollectionDetailTags Tags,

    [property: JsonPropertyName("tiles")]
    List<CollectionTile> Tiles
);

/// <summary>
/// 
/// </summary>
/// <param name="AuthorId"></param>
/// <param name="IsLocked"></param>
/// <param name="Tags"></param>
/// <param name="Writable"></param>
/// <param name="Success"></param>
public record CollectionDetailTags(
    [property: JsonPropertyName("authorId")]
    [property: JsonConverter(typeof(BigIntegerConverter))]
    BigInteger AuthorId,

    [property: JsonPropertyName("isLocked")]
    bool IsLocked,

    [property: JsonPropertyName("tags")]
    List<CollectionTag> Tags,

    [property: JsonPropertyName("writable")]
    bool Writable,

    [property: JsonPropertyName("success")]
    bool Success
);

/// <summary>
/// 
/// </summary>
/// <param name="Tag"></param>
/// <param name="Locked"></param>
/// <param name="Deletable"></param>
/// <param name="UserId"></param>
/// <param name="Romaji"></param>
/// <param name="Translation"></param>
public record CollectionTag(
    [property: JsonPropertyName("tag")]
    string Tag,

    [property: JsonPropertyName("locked")]
    bool Locked,

    [property: JsonPropertyName("deletable")]
    bool Deletable,

    [property: JsonPropertyName("userId")]
    [property: JsonConverter(typeof(BigIntegerConverter))]
    BigInteger UserId,

    [property: JsonPropertyName("romaji")]
    string? Romaji,

    [property: JsonPropertyName("translation")]
    CollectionTagTranslation? Translation
);
