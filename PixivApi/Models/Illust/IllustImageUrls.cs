namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// Image file addresses of different sizes
/// </summary>
/// <param name="Mini">48x48, available when accessed from <see cref="IllustInfo.Urls"/></param>
/// <param name="ThumbInternal">250x250, available when accessed from <see cref="IllustInfo.Urls"/>, use <see cref="Thumb"/> instead</param>
/// <param name="ThumbMiniInternal">250x250, available when accessed from <see cref="IllustImage.Urls"/>, use <see cref="Thumb"/> instead</param>
/// <param name="Small">540x540</param>
/// <param name="Middle">1200x1200</param>
/// <param name="Original">Original image</param>
public record IllustImageUrls(
    [property: JsonPropertyName("mini")]
    string? Mini,

    [property: JsonInclude, JsonPropertyName("thumb")]
    string ThumbInternal,

    [property: JsonInclude, JsonPropertyName("thumb_mini")]
    string ThumbMiniInternal,

    [property: JsonPropertyName("small")]
    string Small,

    [property: JsonPropertyName("regular")]
    string Middle,

    [property: JsonPropertyName("original")]
    string Original
)
{
    /// <summary>
    /// 250x250
    /// </summary>
    [JsonIgnore]
    public string Thumb => ThumbInternal + ThumbMiniInternal;
}
