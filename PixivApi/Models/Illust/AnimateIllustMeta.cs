namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// Animation metadata. An animation on Pixiv is an image sequence packed into a ZIP file. The root directory of the ZIP file contains the image sequence, with filenames like 000000.jpg, 000001.jpg.
/// </summary>
/// <param name="IllustId">Illustration id</param>
/// <param name="SmallUrl">Small size animation ZIP file URL</param>
/// <param name="OriginalUrl">Original size animation ZIP file URL</param>
/// <param name="MimeType">MIME type</param>
/// <param name="Frames">Animation frame information</param>
public record AnimateIllustMeta(
    [property: JsonIgnore]
    int IllustId,

    [property: JsonPropertyName("src")]
    string SmallUrl,

    [property: JsonPropertyName("originalSrc")]
    string OriginalUrl,

    [property: JsonPropertyName("mime_type")]
    string MimeType,

    [property: JsonPropertyName("frames")]
    List<AnimateIllustFrame> Frames
);

/// <summary>
/// Animation frame information, including filename and frame delay
/// </summary>
/// <param name="File">Filename, e.g., 000000.jpg</param>
/// <param name="Delay">Frame delay, in milliseconds</param>
public record AnimateIllustFrame(
    [property: JsonPropertyName("file")]
    string File,

    [property: JsonPropertyName("delay")]
    int Delay
);