using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.Common;

/// <summary>
/// 
/// </summary>
/// <param name="PopularTags"></param>
/// <param name="RecommendTags"></param>
/// <param name="RecommendByTags"></param>
/// <param name="TagTranslation"></param>
/// <param name="MyFavoriteTags"></param>
/// <param name="Thumbnails"></param>
public record GetSearchSuggestionResponse(
    [property: JsonPropertyName("popularTags")]
    PopularTags PopularTags,
    [property: JsonPropertyName("recommendTags")]
    RecommendTags RecommendTags,
    // Novel is null here
    [property: JsonPropertyName("recommendByTags")]
    PopularTags RecommendByTags,
    [property: JsonPropertyName("tagTranslation")] 
    Dictionary<string, TopIllustTagTranslation> TagTranslation,
    [property: JsonPropertyName("myFavoriteTags")]
    string[] MyFavoriteTags,
    [property: JsonPropertyName("thumbnails")]
    List<IllustProfile> Thumbnails
    );
    
/// <summary>
/// 
/// </summary>
/// <param name="Illust"></param>
/// <param name="Novel">Returns null when exploring RecommendedTags and RecommendedByTags</param>
public record PopularTags(
    [property: JsonPropertyName("illust")]
    IllustMangaTags[] Illust,
    [property: JsonPropertyName("novel")]
    NovelTags[]? Novel
    );
    
/// <summary>
/// 
/// </summary>
/// <param name="Illust"></param>
public record RecommendTags(
    [property: JsonPropertyName("illust")]
    NovelTags[] Illust
);