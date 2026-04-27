using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.User;

/// <summary>
/// User's recent works
/// </summary>
/// <param name="Illusts">Illustrations</param>
/// <param name="Mangas">Manga</param>
/// <param name="Novels">Novels</param>
public record UserTopWorks(
    [property: JsonPropertyName("illusts")]
    [property: JsonConverter(typeof(DictionaryValueToListJsonConverterIllustProfile))]
    List<IllustProfile> Illusts,

    [property: JsonPropertyName("manga")]
    [property: JsonConverter(typeof(DictionaryValueToListJsonConverterIllustProfile))]
    List<IllustProfile> Mangas,

    [property: JsonPropertyName("novels")]
    [property: JsonConverter(typeof(DictionaryValueToListJsonConverterNovelProfile))]
    List<NovelProfile> Novels
);
