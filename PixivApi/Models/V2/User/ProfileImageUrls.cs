namespace Scighost.PixivApi.Models.V2.User;

/// <summary>
/// 
/// </summary>
/// <param name="Px16X16"></param>
/// <param name="Px50X50"></param>
/// <param name="Px170X170"></param>
public record ProfileImageUrls(
    [property: JsonPropertyName("px_16x16")]
    string Px16X16,
    [property: JsonPropertyName("px_50x50")]
    string Px50X50,
    [property: JsonPropertyName("px_170x170")]
    string Px170X170
);