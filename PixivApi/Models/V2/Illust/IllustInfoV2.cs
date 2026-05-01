using Scighost.PixivApi.Models.V2.User;

namespace Scighost.PixivApi.Models.V2.Illust;

/// <summary>
/// Represents detailed information about an illustration.
/// </summary>
/// <param name="Id">The unique identifier of the illustration.</param>
/// <param name="Title">The title of the illustration.</param>
/// <param name="Type">The type of the illustration (e.g., "illust", "manga").</param>
/// <param name="ThumbnailUrls">Contains the list of all urls used by the thumbnail.</param>
/// <param name="Caption">The caption or description of the illustration.</param>
/// <param name="Restrict">The restriction level of the illustration.</param>
/// <param name="User">The user who created the illustration.</param>
/// <param name="Tags">The tags associated with the illustration.</param>
/// <param name="Tools">The tools used to create the illustration.</param>
/// <param name="CreateDate">The date and time when the illustration was created.</param>
/// <param name="PageCount">The number of pages in the illustration.</param>
/// <param name="Width">The width of the illustration in pixels.</param>
/// <param name="Height">The height of the illustration in pixels.</param>
/// <param name="SanityLevel">The sanity level rating of the illustration.</param>
/// <param name="XRestrict">The x-restrict level (0 = normal, 1 = R18, 2 = R18G).</param>
/// <param name="Series">The series information if the illustration is part of a series.</param>
/// <param name="OriginalPageUrlData">Contains the url of the original image if the illustration post contains a single page.</param>
/// <param name="MultiPageUrlData">Contains the url of all images if the post contains multiple pages.</param>
/// <param name="TotalView">The total number of views the illustration has received.</param>
/// <param name="TotalBookmarks">The total number of bookmarks the illustration has received.</param>
/// <param name="IsBookmarked">Whether the authenticated user has bookmarked this illustration.</param>
/// <param name="Visible">Whether the illustration is visible.</param>
/// <param name="IsMuted">Whether the illustration is muted.</param>
/// <param name="TotalComments">The total number of comments on the illustration.</param>
/// <param name="IllustAiType">The AI type classification of the illustration.</param>
public record IllustInfoV2(
    [property: JsonPropertyName("id"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,
    [property: JsonPropertyName("title")]
    string Title,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("image_urls")]
    ImageUrls ThumbnailUrls,
    [property: JsonPropertyName("caption")]
    string Caption,
    [property: JsonPropertyName("restrict")]
    int Restrict,
    [property: JsonPropertyName("user")]
    PublicUser User,
    [property: JsonPropertyName("tags")]
    Tag[] Tags,
    [property: JsonPropertyName("tools")]
    string[] Tools,
    [property: JsonPropertyName("create_date")]
    DateTimeOffset CreateDate,
    [property: JsonPropertyName("page_count")]
    int PageCount,
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
    int TotalView,
    [property: JsonPropertyName("total_bookmarks")]
    int TotalBookmarks,
    [property: JsonPropertyName("is_bookmarked")]
    bool IsBookmarked,
    [property: JsonPropertyName("visible")]
    bool Visible,
    [property: JsonPropertyName("is_muted")]
    bool IsMuted,
    [property: JsonPropertyName("total_comments")]
    int TotalComments,
    [property: JsonPropertyName("illust_ai_type")]
    IllustAiType IllustAiType
);
