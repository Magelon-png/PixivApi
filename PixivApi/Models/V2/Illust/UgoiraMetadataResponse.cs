namespace Scighost.PixivApi.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="UgoiraMetadata"></param>
public record UgoiraMetadataResponse(
    [property: JsonPropertyName("ugoira_metadata")]
    UgoiraMetadata UgoiraMetadata
    );
    
/// <summary>
/// 
/// </summary>
/// <param name="Frames"></param>
/// <param name="ZipUrls">Only returns the medium URL?</param>
public record UgoiraMetadata(
    [property: JsonPropertyName("frames")]
    UgoiraFrame[] Frames,
    [property: JsonPropertyName("zip_urls")]
    ImageUrls ZipUrls
    );

/// <summary>
/// 
/// </summary>
/// <param name="File"></param>
/// <param name="Delay"></param>
public record UgoiraFrame(
    [property: JsonPropertyName("file")]
    string File,
    [property: JsonPropertyName("delay")]
    int Delay
);