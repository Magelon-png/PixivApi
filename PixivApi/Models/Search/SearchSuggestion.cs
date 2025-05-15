namespace Scighost.PixivApi.Models.Search;

internal class SearchSuggestion
{

    public List<string> MyFavoriteTags { get; set; }

    // todo




}



internal class SearchRecommendTag
{
    public string Tag { get; set; }

    public List<string> Ids { get; set; }

    // todo
}



/// <summary>
/// 搜索候选词
/// </summary>
public class SearchCandidate
{

    /// <summary>
    /// 不知道什么意思
    /// </summary>
    [JsonPropertyName("access_count")]
    public string AccessCount { get; set; }

    /// <summary>
    /// 标签名
    /// </summary>
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }

    /// <summary>
    /// 标签翻译
    /// </summary>
    [JsonPropertyName("tag_translation")]
    public string? TagTranslation { get; set; }

    /// <summary>
    /// 推荐类型（原因），前缀匹配：prefix，标签翻译：tag_translation
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }
}