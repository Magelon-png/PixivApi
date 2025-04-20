namespace Scighost.PixivApi.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="IllustType"></param>
/// <param name="XRestrict"></param>
/// <param name="Restrict"></param>
/// <param name="Sl"></param>
/// <param name="Url"></param>
/// <param name="Description"></param>
/// <param name="Tags"></param>
/// <param name="UserId"></param>
/// <param name="UserName"></param>
/// <param name="Width"></param>
/// <param name="Height"></param>
/// <param name="PageCount"></param>
/// <param name="IsBookmarkable"></param>
/// <param name="BookmarkData"></param>
/// <param name="Alt"></param>
/// <param name="TitleCaptionTranslation"></param>
/// <param name="CreateDate"></param>
/// <param name="UpdateDate"></param>
/// <param name="IsUnlisted"></param>
/// <param name="IsMasked"></param>
/// <param name="AiType"></param>
/// <param name="VisibilityScope"></param>
/// <param name="ProfileImageUrl"></param>
public record IllustSearchData(
    [property: JsonPropertyName("id"),
               JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
    int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("illustType")] int IllustType,
    [property: JsonPropertyName("xRestrict")] int XRestrict,
    [property: JsonPropertyName("restrict")] int Restrict,
    [property: JsonPropertyName("sl")] int Sl,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("tags")] string[] Tags,
    [property: JsonPropertyName("userId"),
               JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
    int UserId,
    [property: JsonPropertyName("userName")] string UserName,
    [property: JsonPropertyName("width")] int Width,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("pageCount")] int PageCount,
    [property: JsonPropertyName("isBookmarkable")] bool IsBookmarkable,
    [property: JsonPropertyName("bookmarkData")] object BookmarkData,
    [property: JsonPropertyName("alt")] string Alt,
    [property: JsonPropertyName("titleCaptionTranslation")] IllustSearchTitleCaptionTranslation TitleCaptionTranslation,
    [property: JsonPropertyName("createDate")] DateTimeOffset CreateDate,
    [property: JsonPropertyName("updateDate")] DateTimeOffset UpdateDate,
    [property: JsonPropertyName("isUnlisted")] bool IsUnlisted,
    [property: JsonPropertyName("isMasked")] bool IsMasked,
    [property: JsonPropertyName("aiType")] int AiType,
    [property: JsonPropertyName("visibilityScope")] int VisibilityScope,
    [property: JsonPropertyName("profileImageUrl")] string ProfileImageUrl
);