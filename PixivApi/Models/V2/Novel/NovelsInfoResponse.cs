using System.Text.Json.Serialization;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.V2.Novel;

/// <summary>
/// 
/// </summary>
/// <param name="Novels"></param>
/// <param name="NextUrl"></param>
public record NovelsInfoResponse(
    [property: JsonPropertyName("novels")]
    List<NovelProfile> Novels,
    [property: JsonPropertyName("next_url")]
    string? NextUrl);
