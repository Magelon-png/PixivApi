using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Illusts"></param>
/// <param name="NextUrl"></param>
public record IllustsInfoResponse(
    [property: JsonPropertyName("illusts")]
    List<IllustInfoV2> Illusts,
    [property: JsonPropertyName("next_url")]
    string NextUrl);