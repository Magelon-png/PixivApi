namespace Scighost.PixivApi.Models.Search;

internal sealed record SearchSuggestion(
    List<string> MyFavoriteTags
);


internal sealed record SearchRecommendTag(
    string Tag,
    List<string> Ids
);


/// <summary>
/// Search candidates
/// </summary>
/// <param name="AccessCount">Unknown meaning</param>
/// <param name="TagName">Tag name</param>
/// <param name="TagTranslation">Tag translation</param>
/// <param name="Type">Recommendation type (reason), prefix match: prefix, tag translation: tag_translation</param>
public record SearchCandidate(
    [property: JsonPropertyName("access_count")]
    string AccessCount,

    [property: JsonPropertyName("tag_name")]
    string TagName,

    [property: JsonPropertyName("tag_translation")]
    string? TagTranslation,

    [property: JsonPropertyName("type")]
    string Type
);