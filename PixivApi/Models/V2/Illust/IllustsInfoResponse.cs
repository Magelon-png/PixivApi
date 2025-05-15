using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Illusts"></param>
public record IllustsInfoResponse(
    [property: JsonPropertyName("illusts")]
    List<IllustInfo> Illusts,
    [property: JsonPropertyName("next_url")]
    string NextUrl);