using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// All fields for illustration and manga details
/// </summary>
[Obsolete("Not used.")]
internal record IllustInfoResponse(
    [property: JsonPropertyName("illustId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int IllustId,

    [property: JsonPropertyName("illustTitle")]
    string IllustTitle,

    [property: JsonPropertyName("illustComment")]
    string IllustComment,

    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("illustType")]
    IllustType IllustType,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("uploadDate")]
    DateTimeOffset UploadDate,

    [property: JsonPropertyName("restrict")]
    int Restrict,

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("sl")]
    int Sl,

    [property: JsonPropertyName("urls")]
    IllustImageUrls Urls,

    [property: JsonPropertyName("tags")]
    PixivTagInternal Tags,

    [property: JsonPropertyName("alt")]
    string Alt,

    [property: JsonPropertyName("storableTags")]
    List<string> StorableTags,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("userAccount")]
    string UserAccount,

    [property: JsonPropertyName("userIllusts")]
    Dictionary<int, IllustProfile?> UserIllusts,

    [property: JsonPropertyName("likeData")]
    bool LikeData,

    [property: JsonPropertyName("width")]
    int Width,

    [property: JsonPropertyName("height")]
    int Height,

    [property: JsonPropertyName("pageCount")]
    int PageCount,

    [property: JsonPropertyName("bookmarkCount")]
    int BookmarkCount,

    [property: JsonPropertyName("likeCount")]
    int LikeCount,

    [property: JsonPropertyName("commentCount")]
    int CommentCount,

    [property: JsonPropertyName("responseCount")]
    int ResponseCount,

    [property: JsonPropertyName("viewCount")]
    int ViewCount,

    [property: JsonPropertyName("bookStyle")]
    int BookStyle,

    [property: JsonPropertyName("isHowto")]
    bool IsHowto,

    [property: JsonPropertyName("isOriginal")]
    bool IsOriginal,

    [property: JsonPropertyName("imageResponseOutData")]
    List<object> ImageResponseOutData,

    [property: JsonPropertyName("imageResponseData")]
    List<object> ImageResponseData,

    [property: JsonPropertyName("imageResponseCount")]
    int ImageResponseCount,

    [property: JsonPropertyName("pollData")]
    object PollData,

    [property: JsonPropertyName("seriesNavData")]
    object SeriesNavData,

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

    [property: JsonPropertyName("isBookmarkable")]
    bool IsBookmarkable,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData,

    [property: JsonPropertyName("contestData")]
    object ContestData,

    [property: JsonPropertyName("zoneConfig")]
    object ZoneConfig,

    [property: JsonPropertyName("extraData")]
    object ExtraData,

    [property: JsonPropertyName("titleCaptionTranslation")]
    object TitleCaptionTranslation,

    [property: JsonPropertyName("isUnlisted")]
    bool IsUnlisted,

    [property: JsonPropertyName("request")]
    object Request,

    [property: JsonPropertyName("commentOff")]
    int CommentOff
);
