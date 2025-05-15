namespace Scighost.PixivApi.V2.Illust;

public record IllustInfoResponse(
    [property: JsonPropertyName("illust")]
    IllustInfo Illust
    );