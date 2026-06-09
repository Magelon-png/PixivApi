using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="Illusts"></param>
/// <param name="Novel"></param>
/// <param name="NovelSeries"></param>
/// <param name="NovelDraft"></param>
/// <param name="Collection"></param>
public record CollectionThumbnails(
    [property: JsonPropertyName("illust")]
    List<IllustProfile> Illusts,

    [property: JsonPropertyName("novel")]
    List<NovelProfileHomePage> Novel,

    [property: JsonPropertyName("novelSeries")]
    List<NovelSeries> NovelSeries,

    [property: JsonPropertyName("novelDraft")]
    List<JsonNode> NovelDraft,

    [property: JsonPropertyName("collection")]
    List<CollectionProfile> Collection
);
