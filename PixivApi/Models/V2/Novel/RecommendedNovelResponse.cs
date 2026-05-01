using System.Text.Json.Serialization;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.V2.Novel;

/// <summary>
/// 
/// </summary>
/// <param name="Novels"></param>
/// <param name="NextUrl"></param>
/// <param name="PrivacyPolicy"></param>
public record RecommendedNovelResponse(
    [property: JsonPropertyName("novels")]
    List<NovelProfile> Novels,
    [property: JsonPropertyName("next_url")]
    string? NextUrl,
    [property: JsonPropertyName("privacy_policy")]
    object? PrivacyPolicy);
