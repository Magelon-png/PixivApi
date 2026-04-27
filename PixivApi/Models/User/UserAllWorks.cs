using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.User;

/// <summary>
/// 
/// </summary>
/// <param name="Illusts">Illustration work ids</param>
/// <param name="Manga">Manga work ids</param>
/// <param name="Novels">Novel work ids</param>
/// <param name="MangaSeries">Manga series</param>
/// <param name="NovelSeries">Novel series</param>
/// <param name="Pickup">Featured works, too complex structure, not represented by entity classes</param>
public record UserAllWorks(
    [property: JsonPropertyName("illusts")]
    [property: JsonConverter(typeof(DictionaryKeyToListJsonConverterInt32))]
    List<int> Illusts,

    [property: JsonPropertyName("manga")]
    [property: JsonConverter(typeof(DictionaryKeyToListJsonConverterInt32))]
    List<int> Manga,

    [property: JsonPropertyName("novels")]
    [property: JsonConverter(typeof(DictionaryKeyToListJsonConverterInt32))]
    List<int> Novels,

    [property: JsonPropertyName("mangaSeries")]
    List<MangaSeries> MangaSeries,

    [property: JsonPropertyName("novelSeries")]
    List<NovelSeries> NovelSeries,

    [property: JsonPropertyName("pickup")]
    List<JsonNode> Pickup
);