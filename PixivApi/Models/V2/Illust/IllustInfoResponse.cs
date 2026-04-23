using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Illust"></param>
public record IllustInfoResponse(
    [property: JsonPropertyName("illust")]
    IllustInfoV2 Illust
    );