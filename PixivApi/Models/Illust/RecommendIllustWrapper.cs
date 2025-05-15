namespace Scighost.PixivApi.Illust;


internal sealed class RecommendIllustWrapper
{

    [JsonPropertyName("illusts")]
    public List<IllustProfile> Illusts { get; set; }


    [JsonPropertyName("nextIds")]
    public List<string> NextIds { get; set; }

}


