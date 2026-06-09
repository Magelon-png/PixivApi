using System.Numerics;
using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Collection;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="UserId"></param>
/// <param name="UserName"></param>
/// <param name="ProfileImageUrl"></param>
/// <param name="Title"></param>
/// <param name="Tags"></param>
/// <param name="Caption"></param>
/// <param name="Language"></param>
/// <param name="VisibilityScope"></param>
/// <param name="XRestrict"></param>
/// <param name="Sl"></param>
/// <param name="CommentOff"></param>
/// <param name="IsSpoiler"></param>
/// <param name="IsBookmarkable"></param>
/// <param name="BookmarkData"></param>
/// <param name="BookmarkCount"></param>
/// <param name="ViewCount"></param>
/// <param name="CitedDataHash"></param>
/// <param name="ThumbnailImageUrl"></param>
/// <param name="Status"></param>
/// <param name="PublishedDateTime"></param>
public record CollectionProfile(
    [property: JsonPropertyName("id")]
    [property: JsonConverter(typeof(BigIntegerConverter))]
    BigInteger Id,

    [property: JsonPropertyName("userId")]
    [property: JsonConverter(typeof(BigIntegerConverter))]
    BigInteger UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("profileImageUrl")]
    string ProfileImageUrl,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("tags")]
    List<string> Tags,

    [property: JsonPropertyName("caption")]
    string Caption,

    [property: JsonPropertyName("language")]
    string Language,

    [property: JsonPropertyName("visibilityScope")]
    int VisibilityScope,

    [property: JsonPropertyName("xRestrict")]
    int XRestrict,

    [property: JsonPropertyName("sl")]
    int Sl,

    [property: JsonPropertyName("commentOff")]
    bool CommentOff,

    [property: JsonPropertyName("isSpoiler")]
    bool IsSpoiler,

    [property: JsonPropertyName("isBookmarkable")]
    bool IsBookmarkable,

    [property: JsonPropertyName("bookmarkData")]
    BookmarkData? BookmarkData,

    [property: JsonPropertyName("bookmarkCount")]
    int BookmarkCount,

    [property: JsonPropertyName("viewCount")]
    int ViewCount,

    [property: JsonPropertyName("citedDataHash")]
    string CitedDataHash,

    [property: JsonPropertyName("thumbnailImageUrl")]
    string ThumbnailImageUrl,

    [property: JsonPropertyName("status")]
    string Status,

    [property: JsonPropertyName("publishedDateTime")]
    string PublishedDateTime
);
