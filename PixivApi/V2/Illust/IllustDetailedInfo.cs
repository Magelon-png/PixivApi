using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.V2.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Type"></param>
/// <param name="ThumbnailUrls">Contains the list of all urls used by the thumbnail</param>
/// <param name="Caption"></param>
/// <param name="Restrict"></param>
/// <param name="User"></param>
/// <param name="Tags"></param>
/// <param name="Tools"></param>
/// <param name="CreateDate"></param>
/// <param name="PageCount"></param>
/// <param name="Width"></param>
/// <param name="Height"></param>
/// <param name="SanityLevel"></param>
/// <param name="XRestrict"></param>
/// <param name="Series"></param>
/// <param name="OriginalPageUrlData">Contains the url of the original image if the illustration post contains a single page</param>
/// <param name="MultiPageUrlData">Contains the url of all images if the post contains multiple pages</param>
/// <param name="TotalView"></param>
/// <param name="TotalBookmarks"></param>
/// <param name="IsBookmarked"></param>
/// <param name="Visible"></param>
/// <param name="IsMuted"></param>
public record IllustDetailedInfo(
    [property: JsonPropertyName("id"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    uint Id,
    [property: JsonPropertyName("title")]
    string Title,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("image_urls")]
    ImageUrls ThumbnailUrls,
    [property: JsonPropertyName("caption")]
    string Caption,
    [property: JsonPropertyName("restrict")]
    uint Restrict,
    [property: JsonPropertyName("user")]
    PublicUser User,
    [property: JsonPropertyName("tags")]
    Tag[] Tags,
    [property: JsonPropertyName("tools")]
    string[] Tools,
    [property: JsonPropertyName("create_date")]
    DateTimeOffset CreateDate,
    [property: JsonPropertyName("page_count")]
    uint PageCount,
    [property: JsonPropertyName("width")]
    int Width,
    [property: JsonPropertyName("height")]
    int Height,
    [property: JsonPropertyName("sanity_level")]
    int SanityLevel,
    [property: JsonPropertyName("x_restrict")]
    int XRestrict,
    [property: JsonPropertyName("series")]
    Series Series,
    [property: JsonPropertyName("meta_single_page")]
    MetaSinglePage OriginalPageUrlData,
    [property: JsonPropertyName("meta_pages")]
    MetaPages[] MultiPageUrlData,
    [property: JsonPropertyName("total_view")]
    uint TotalView,
    [property: JsonPropertyName("total_bookmarks")]
    uint TotalBookmarks,
    [property: JsonPropertyName("is_bookmarked")]
    bool IsBookmarked,
    [property: JsonPropertyName("visible")]
    bool Visible,
    [property: JsonPropertyName("is_muted")]
    bool IsMuted,
    [property: JsonPropertyName("total_comments")]
    uint TotalComments,
    [property: JsonPropertyName("illust_ai_type")]
    IllustAiType IllustAiType
);

[EnumExtensions]
public enum IllustAiType
{
    [Display(Name="Not AI")]
    NotAi = 0 | 1,
    [Display]
    Ai = 2
}