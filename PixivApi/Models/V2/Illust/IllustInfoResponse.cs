using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Illust;

public record IllustInfoResponse(
    [property: JsonPropertyName("illust")]
    IllustInfo Illust
    );