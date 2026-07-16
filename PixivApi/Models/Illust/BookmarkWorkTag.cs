namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Tag">The illust tag that can be used as filter</param>
/// <param name="Count">How many illusts possess this tag</param>
/// <param name="Translation">The translation of the tag</param>
public record BookmarkWorkTag(
    [property: JsonPropertyName("tag_name")]
    string Tag,
    [property: JsonPropertyName("count")]
    int Count,
    [property: JsonPropertyName("tag_translation")]
    string Translation
    );

/// <summary>
/// Represents a wrapper that contains a list of bookmark work tag candidates.
/// This class is typically used to organize and handle tag information
/// associated with illustrations that can be filtered based on specific tags.
/// </summary>
/// <param name="Candidates">A list of bookmark work tag candidates</param>
public record BookmarkWorkTagWrapper(
    [property: JsonPropertyName("candidates")]
    List<BookmarkWorkTag> Candidates
    );