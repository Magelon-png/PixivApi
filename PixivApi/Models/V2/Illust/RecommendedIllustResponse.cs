using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Illust;

public record RecommendedIllustResponse(
    [property: JsonPropertyName("home_ranking_illusts")]
    List<IllustInfo> HomeRankingIllusts,
    [property: JsonPropertyName("illusts")]
    List<IllustInfo> Illusts,
    [property: JsonPropertyName("next_url")]
    string NextUrl,
    [property: JsonPropertyName("ranking_label_illust")]
    IllustLabel RankingLabelIllust
    );
    
public record IllustLabel(
    [property: JsonPropertyName("title")]
    string Title,
    [property: JsonPropertyName("width")]
    int Width,
    [property: JsonPropertyName("height")]
    int Height,
    [property: JsonPropertyName("user_name")]
    string UserName,
    [property: JsonPropertyName("image_urls")]
    ImageUrls ImageUrls
    );