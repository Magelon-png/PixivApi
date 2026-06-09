using System.Numerics;
using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Layout"></param>
/// <param name="Status"></param>
/// <param name="Type"></param>
/// <param name="WorkType"></param>
/// <param name="WorkId"></param>
/// <param name="QuotePage"></param>
/// <param name="Text"></param>
/// <param name="Background"></param>
/// <param name="TextSize"></param>
/// <param name="Align"></param>
/// <param name="FontType"></param>
/// <param name="Url"></param>
/// <param name="SkipJumpPage"></param>
/// <param name="XRestrict"></param>
/// <param name="ShowPreview"></param>
/// <param name="Title"></param>
/// <param name="ImageUrl"></param>
/// <param name="IconUrl"></param>
/// <param name="IconName"></param>
/// <param name="Description"></param>
public record CollectionTile(
    [property: JsonPropertyName("id")]
    string Id,

    [property: JsonPropertyName("layout")]
    CollectionTileLayout Layout,

    [property: JsonPropertyName("status")]
    string Status,

    [property: JsonPropertyName("type")]
    string Type,

    [property: JsonPropertyName("workType")]
    string? WorkType,

    [property: JsonPropertyName("workId")]
    [property: JsonConverter(typeof(BigIntegerConverter))]
    BigInteger? WorkId,

    [property: JsonPropertyName("quotePage")]
    JsonNode? QuotePage,

    [property: JsonPropertyName("text")]
    string? Text,

    [property: JsonPropertyName("background")]
    string? Background,

    [property: JsonPropertyName("textSize")]
    string? TextSize,

    [property: JsonPropertyName("align")]
    string? Align,

    [property: JsonPropertyName("fontType")]
    string? FontType,

    [property: JsonPropertyName("url")]
    string? Url,

    [property: JsonPropertyName("skipJumpPage")]
    bool? SkipJumpPage,

    [property: JsonPropertyName("xRestrict")]
    int? XRestrict,

    [property: JsonPropertyName("showPreview")]
    bool? ShowPreview,

    [property: JsonPropertyName("title")]
    string? Title,

    [property: JsonPropertyName("imageUrl")]
    string? ImageUrl,

    [property: JsonPropertyName("iconUrl")]
    string? IconUrl,

    [property: JsonPropertyName("iconName")]
    string? IconName,

    [property: JsonPropertyName("description")]
    string? Description
);

/// <summary>
/// 
/// </summary>
/// <param name="Position"></param>
/// <param name="Size"></param>
public record CollectionTileLayout(
    [property: JsonPropertyName("position")]
    CollectionTilePosition Position,

    [property: JsonPropertyName("size")]
    CollectionTilePosition Size
);

/// <summary>
/// 
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public record CollectionTilePosition(
    [property: JsonPropertyName("x")]
    int X,

    [property: JsonPropertyName("y")]
    int Y
);
