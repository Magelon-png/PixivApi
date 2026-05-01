using System.Text.Json.Serialization;
using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Common;

/// <summary>
/// 
/// </summary>
/// <param name="TrendTags"></param>
public record TrendingTagsResponse(
    [property: JsonPropertyName("trend_tags")]
    List<TrendingTag> TrendTags);

/// <summary>
/// 
/// </summary>
/// <param name="Tag"></param>
/// <param name="TranslatedName"></param>
/// <param name="Illust"></param>
public record TrendingTag(
    [property: JsonPropertyName("tag")]
    string Tag,
    [property: JsonPropertyName("translated_name")]
    string? TranslatedName,
    [property: JsonPropertyName("illust")]
    IllustInfo Illust);
