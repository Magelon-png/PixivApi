using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.Common;

internal class Thumbnails
{
    [JsonPropertyName("illust")]
    public List<IllustProfile> Illusts { get; set; }

    [JsonPropertyName("novel")]
    public List<NovelProfile> Novels { get; set; }
}