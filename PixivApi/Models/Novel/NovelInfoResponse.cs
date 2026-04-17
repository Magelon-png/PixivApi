using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// All fields for novel details
/// </summary>
[Obsolete("Not used.")]
internal record NovelInfoResponse(
    [property: JsonPropertyName("bookmarkCount")]
    int BookmarkCount,

    [property: JsonPropertyName("commentCount")]
    int CommentCount,

    [property: JsonPropertyName("markerCount")]
    int MarkerCount,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("uploadDate")]
    DateTimeOffset UploadDate,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("likeCount")]
    int LikeCount,

    [property: JsonPropertyName("pageCount")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int PageCount,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("viewCount")]
    int ViewCount,

    [property: JsonPropertyName("isOriginal")]
    bool IsOriginal,

    [property: JsonPropertyName("isBungei")]
    bool IsBungei,

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("restrict")]
    int Restrict,

    [property: JsonPropertyName("content")]
    string Content,

    [property: JsonPropertyName("coverUrl")]
    string CoverUrl,

    [property: JsonPropertyName("suggestedSettings")]
    object SuggestedSettings,

    [property: JsonPropertyName("isBookmarkable")]
    bool IsBookmarkable,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData,

    [property: JsonPropertyName("likeData")]
    bool LikeData,

    [property: JsonPropertyName("pollData")]
    object PollData,

    /// <summary>
    /// Which page of the article was bookmarked
    /// </summary>
    [property: JsonPropertyName("marker")]
    int? Marker,

    [property: JsonPropertyName("tags")]
    PixivTagInternal Tags,

    [property: JsonPropertyName("seriesNavData")]
    SeriesNavData SeriesNavData,

    [property: JsonPropertyName("descriptionBoothId")]
    object DescriptionBoothId,

    [property: JsonPropertyName("descriptionYoutubeId")]
    object DescriptionYoutubeId,

    [property: JsonPropertyName("comicPromotion")]
    object ComicPromotion,

    [property: JsonPropertyName("fanboxPromotion")]
    object FanboxPromotion,

    [property: JsonPropertyName("contestBanners")]
    List<object> ContestBanners,

    [property: JsonPropertyName("contestData")]
    object ContestData,

    [property: JsonPropertyName("request")]
    object Request,

    [property: JsonPropertyName("imageResponseOutData")]
    List<object> ImageResponseOutData,

    [property: JsonPropertyName("imageResponseData")]
    List<object> ImageResponseData,

    [property: JsonPropertyName("imageResponseCount")]
    int ImageResponseCount,

    [property: JsonPropertyName("userNovels")]
    Dictionary<int, NovelProfile?> UserNovels,

    [property: JsonPropertyName("hasGlossary")]
    bool HasGlossary,

    [property: JsonPropertyName("zoneConfig")]
    object ZoneConfig,

    [property: JsonPropertyName("extraData")]
    object ExtraData,

    [property: JsonPropertyName("titleCaptionTranslation")]
    object TitleCaptionTranslation,

    [property: JsonPropertyName("isUnlisted")]
    bool IsUnlisted,

    [property: JsonPropertyName("language")]
    string Language,

    [property: JsonPropertyName("textEmbeddedImages")]
    object TextEmbeddedImages,

    [property: JsonPropertyName("commentOff")]
    int CommentOff,

    [property: JsonPropertyName("characterCount")]
    int CharacterCount,

    [property: JsonPropertyName("wordCount")]
    int WordCount,

    [property: JsonPropertyName("useWordCount")]
    bool UseWordCount,

    [property: JsonPropertyName("readingTime")]
    int ReadingTime
);
