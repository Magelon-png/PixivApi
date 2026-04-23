using System.Text.Json.Serialization;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.V2.Novel;

/// <summary>
/// 
/// </summary>
/// <param name="Novel"></param>
public record NovelInfoResponse(
    [property: JsonPropertyName("novel")]
    NovelInfo Novel);
