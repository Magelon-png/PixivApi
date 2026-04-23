using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="IllustType"></param>
/// <param name="CreateDate"></param>
/// <param name="UploadDate"></param>
/// <param name="XRestrict"></param>
/// <param name="Urls"></param>
/// <param name="Tags"></param>
/// <param name="UserId"></param>
/// <param name="UserName"></param>
/// <param name="UserAccount"></param>
/// <param name="UserIllusts"></param>
/// <param name="IsLike"></param>
/// <param name="Width"></param>
/// <param name="Height"></param>
/// <param name="PageCount"></param>
/// <param name="BookmarkCount"></param>
/// <param name="LikeCount"></param>
/// <param name="CommentCount"></param>
/// <param name="ResponseCount"></param>
/// <param name="ViewCount"></param>
/// <param name="IsOriginal"></param>
/// <param name="SeriesNavData"></param>
/// <param name="BookmarkData"></param>
[JsonSerializable(typeof(IllustInfo), TypeInfoPropertyName = "IllustInfoV1")]
public record IllustInfo(
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

    [property: JsonPropertyName("xRestrict")]
    XRestrict XRestrict,

    [property: JsonPropertyName("urls")]
    IllustImageUrls Urls,

    [property: JsonPropertyName("tags")]
    [property: JsonConverter(typeof(PixivTagJsonConverter))]
    List<PixivTag> Tags,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("userAccount")]
    string UserAccount,

    [property: JsonPropertyName("userIllusts")]
    [property: JsonConverter(typeof(DictionaryKeyToListJsonConverter<int>))]
    List<int> UserIllusts,

    [property: JsonPropertyName("likeData")]
    bool IsLike,

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

    [property: JsonPropertyName("isOriginal")]
    bool IsOriginal,

    [property: JsonPropertyName("seriesNavData")]
    SeriesNavData? SeriesNavData,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData
);
