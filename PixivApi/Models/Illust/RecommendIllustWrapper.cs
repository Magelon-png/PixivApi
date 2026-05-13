namespace Scighost.PixivApi.Models.Illust;


internal sealed class RecommendIllustWrapper
{

    [JsonPropertyName("illusts")]
    public List<IllustProfile> Illusts { get; set; }


    [JsonPropertyName("nextIds")]
    public List<string> NextIds { get; set; }

    public RecommendIllustWrapper(List<IllustProfile> illusts, List<string> nextIds)
    {
        Illusts = illusts;
        NextIds = nextIds;
    }
}


