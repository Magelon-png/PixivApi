namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="WorkTitle"></param>
/// <param name="WorkCaption"></param>
public record IllustSearchTitleCaptionTranslation(
    [property: JsonPropertyName("workTitle")] string WorkTitle,
    [property: JsonPropertyName("workCaption")] string WorkCaption
);