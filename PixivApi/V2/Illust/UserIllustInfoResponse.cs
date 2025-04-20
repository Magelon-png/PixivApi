namespace Scighost.PixivApi.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Illusts"></param>
public record UserIllustInfoResponse(
    [property: JsonPropertyName("illusts")]
    List<IllustInfo> Illusts,
    [property: JsonPropertyName("next_url")]
    string NextUrl);