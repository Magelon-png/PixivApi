using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.Common;

internal record Thumbnails(
    [property: JsonPropertyName("illust")]
    List<IllustProfile> Illusts,

    [property: JsonPropertyName("novel")]
    List<NovelProfile> Novels
);