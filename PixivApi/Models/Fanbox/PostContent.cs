namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="Images">The images in the post. Can sometimes be replaced by ImageMap</param>
/// <param name="ImageMap">If the post contains images seperated by text, this property will be populated instead of Images</param>
/// <param name="Files">If the post has files attached (ie: a zip), you will retrieve these in this property</param>
public record PostContent(
    [property: JsonPropertyName("text")]
    string? Text,
    [property: JsonPropertyName("images")]
    PostImage[]? Images,
    [property: JsonPropertyName("imageMap")]
    Dictionary<string, PostImage>? ImageMap,
    [property: JsonPropertyName("fileMap")]
    Dictionary<string, PostFile>? FileMap,
    [property: JsonPropertyName("blocks")]
    PostBlock[]? Blocks,
    [property: JsonPropertyName("files")]
    PostFile[]? Files
);