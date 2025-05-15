namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="OriginalImageUrl"></param>
public record MetaSinglePage(
    [property: JsonPropertyName("original_image_url")]
    string OriginalImageUrl
);