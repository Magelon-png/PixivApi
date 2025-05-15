namespace Scighost.PixivApi.Models.Illust;

internal class BookmarkIllustWrapper
{

    [JsonPropertyName("total")]
    public int Total { get; set; }


    [JsonPropertyName("works")]
    public List<IllustProfile> Works { get; set; }

}
