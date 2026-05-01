using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Illust"></param>
/// <param name="NextUrl"></param>
public record IllustInfoResponse(
    [property: JsonPropertyName("illust")]
    IllustInfoV2 Illust,
    [property: JsonPropertyName("next_url")]
    string? NextUrl
    );
    