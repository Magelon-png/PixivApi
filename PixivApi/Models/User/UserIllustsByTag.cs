using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.User;

/// <summary>
/// 
/// </summary>
/// <param name="Works"></param>
/// <param name="Total"></param>
/// <param name="ExtraData"></param>
public record UserIllustsByTag(
    [property: JsonPropertyName("works")]
    IllustSearchData[] Works,
    [property: JsonPropertyName("total")]
    int Total,
    [property: JsonPropertyName("extraData")]
    IllustSearchExtraData ExtraData
);


