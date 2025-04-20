namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
public class IllustWorks
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("works")]
    public Dictionary<string, IllustWorksInfo> Works { get; set; }
}