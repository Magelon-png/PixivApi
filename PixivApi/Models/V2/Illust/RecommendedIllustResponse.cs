using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="HomeRankingIllusts"></param>
/// <param name="Illusts"></param>
/// <param name="NextUrl"></param>
/// <param name="RankingLabelIllust"></param>
public record RecommendedIllustResponse(
    [property: JsonPropertyName("home_ranking_illusts")]
    List<IllustInfoV2> HomeRankingIllusts,
    [property: JsonPropertyName("illusts")]
    List<IllustInfoV2> Illusts,
    [property: JsonPropertyName("next_url")]
    string NextUrl,
    [property: JsonPropertyName("ranking_label_illust")]
    IllustLabel RankingLabelIllust
    );
    
/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Width"></param>
/// <param name="Height"></param>
/// <param name="UserName"></param>
/// <param name="ImageUrls"></param>
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