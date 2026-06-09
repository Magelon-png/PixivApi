namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="Meta"></param>
public record CollectionExtraData(
    [property: JsonPropertyName("meta")]
    CollectionMeta Meta
);

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Canonical"></param>
/// <param name="Ogp"></param>
/// <param name="Twitter"></param>
/// <param name="Robots"></param>
public record CollectionMeta(
    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("canonical")]
    string Canonical,

    [property: JsonPropertyName("ogp")]
    OgpMeta Ogp,

    [property: JsonPropertyName("twitter")]
    TwitterMeta Twitter,

    [property: JsonPropertyName("robots")]
    string? Robots
);

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Canonical"></param>
/// <param name="Ogp"></param>
/// <param name="Twitter"></param>
/// <param name="AlternateLanguages"></param>
/// <param name="DescriptionHeader"></param>
public record CollectionMetaWithAlternateLanguages(
    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("canonical")]
    string Canonical,

    [property: JsonPropertyName("ogp")]
    OgpMeta Ogp,

    [property: JsonPropertyName("twitter")]
    TwitterMeta Twitter,

    [property: JsonPropertyName("alternateLanguages")]
    Dictionary<string, string> AlternateLanguages,

    [property: JsonPropertyName("descriptionHeader")]
    string DescriptionHeader
);

/// <summary>
/// 
/// </summary>
/// <param name="Type"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Image"></param>
public record OgpMeta(
    [property: JsonPropertyName("type")]
    string Type,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("image")]
    string Image
);

/// <summary>
/// 
/// </summary>
/// <param name="Card"></param>
/// <param name="Site"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Image"></param>
public record TwitterMeta(
    [property: JsonPropertyName("card")]
    string Card,

    [property: JsonPropertyName("site")]
    string? Site,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("image")]
    string Image
);
