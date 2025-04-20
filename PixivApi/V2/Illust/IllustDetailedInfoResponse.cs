namespace Scighost.PixivApi.V2.Illust;

public record IllustDetailedInfoResponse(
    [property: JsonPropertyName("illust")]
    IllustDetailedInfo Illust
    );