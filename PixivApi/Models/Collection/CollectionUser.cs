using System.Numerics;
using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="Partial"></param>
/// <param name="Comment"></param>
/// <param name="FollowedBack"></param>
/// <param name="UserId"></param>
/// <param name="Name"></param>
/// <param name="Image"></param>
/// <param name="ImageBig"></param>
/// <param name="Premium"></param>
/// <param name="IsFollowed"></param>
/// <param name="IsMypixiv"></param>
/// <param name="IsBlocking"></param>
/// <param name="Background"></param>
/// <param name="Commission"></param>
public record CollectionUser(
    [property: JsonPropertyName("partial")]
    int Partial,

    [property: JsonPropertyName("comment")]
    string Comment,

    [property: JsonPropertyName("followedBack")]
    bool FollowedBack,

    [property: JsonPropertyName("userId")]
    [property: JsonConverter(typeof(BigIntegerConverter))]
    BigInteger UserId,

    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("image")]
    string Image,

    [property: JsonPropertyName("imageBig")]
    string ImageBig,

    [property: JsonPropertyName("premium")]
    bool Premium,

    [property: JsonPropertyName("isFollowed")]
    bool IsFollowed,

    [property: JsonPropertyName("isMypixiv")]
    bool IsMypixiv,

    [property: JsonPropertyName("isBlocking")]
    bool IsBlocking,

    [property: JsonPropertyName("background")]
    JsonNode? Background,

    [property: JsonPropertyName("commission")]
    JsonNode? Commission
);
