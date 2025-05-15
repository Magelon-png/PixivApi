namespace Scighost.PixivApi.Models.V2.Illust;



/// <summary>
/// 
/// </summary>
/// <param name="Original">URL for the original size image</param>
/// <param name="Large">URL for the large size image (usually 600x1200)</param>
/// <param name="Medium">URL for the medium size image (usually 540x540)</param>
/// <param name="Small">URL for the small size image</param>
/// <param name="SquareMedium">URL for the square medium size image (usually 360x360)</param>
/// <param name="Max240x240">URL for max 240x240 size image</param>
/// <param name="Px120x">URL for 120x size image</param>
/// <param name="Px128x128">URL for 128x128 size image</param>
/// <param name="Px16x16">URL for 16x16 size image</param>
/// <param name="Px170x170">URL for 170x170 size image</param>
/// <param name="Px480mw">URL for 480mw size image</param>
/// <param name="Px48x48">URL for 48x48 size image</param>
/// <param name="Px50x50">URL for 50x50 size image</param>
/// <param name="Px56x56">URL for 56x56 size image</param>
/// <param name="Px64x64">URL for 64x64 size image</param>
/// <param name="Ugoira600x600">URL for ugoira 600x600 size image</param>
/// <param name="Ugoira1920x1080">URL for ugoira 1920x1080 size image</param>
public record ImageUrls(
    [property: JsonPropertyName("original")]
    string Original,
    [property: JsonPropertyName("large")]
    string Large,
    [property: JsonPropertyName("medium")]
    string Medium,
    [property: JsonPropertyName("small")]
    string Small,
    [property: JsonPropertyName("square_medium")]
    string SquareMedium,
    [property: JsonPropertyName("max_240x240")]
    string Max240x240,
    [property: JsonPropertyName("px_120x")]
    string Px120x,
    [property: JsonPropertyName("px_128x128")]
    string Px128x128,
    [property: JsonPropertyName("px_16x16")]
    string Px16x16,
    [property: JsonPropertyName("px_170x170")]
    string Px170x170,
    [property: JsonPropertyName("px_480mw")]
    string Px480mw,
    [property: JsonPropertyName("px_48x48")]
    string Px48x48,
    [property: JsonPropertyName("px_50x50")]
    string Px50x50,
    [property: JsonPropertyName("px_56x56")]
    string Px56x56,
    [property: JsonPropertyName("px_64x64")]
    string Px64x64,
    [property: JsonPropertyName("ugoira600x600")]
    string Ugoira600x600,
    [property: JsonPropertyName("ugoira1920x1080")]
    string Ugoira1920x1080
);