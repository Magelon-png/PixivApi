namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Type">p = text, image = image, header = text header, file = a file</param>
/// <param name="Text">Populated if type is p or header</param>
/// <param name="ImageId">Populated if type is image. To be used as a key for PostContent.ImageMap</param>
/// <param name="FileId">Populated if type is file</param>
public record PostBlock(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("text")] string? Text,
    [property: JsonPropertyName("imageId")]
    string? ImageId, 
    [property: JsonPropertyName("fileId")]
    string? FileId);