using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;

namespace Scighost.PixivApi.Models.Common;

/// <summary>
/// 
/// </summary>
/// <param name="Illusts"></param>
/// <param name="Novels"></param>
public record Thumbnails(
    [property: JsonPropertyName("illust")]
    List<IllustProfile> Illusts,

    [property: JsonPropertyName("novel")]
    List<NovelProfile> Novels
);

///<inheritdoc cref="Thumbnails"/>
/// <summary>
/// 
/// </summary>
/// <param name="Illusts"></param>
/// <param name="Novels"></param>
/// <param name="NovelSeries"></param>
/// <param name="NovelDraft"></param>
/// <param name="Collection"></param>
public sealed record ThumbnailsIllustHomePage(
    [property: JsonPropertyName("illust")]
    List<IllustProfile> Illusts,

    [property: JsonPropertyName("novel")]
    List<NovelProfileHomePage> Novels,
    [property: JsonPropertyName("novelSeries")] object[]? NovelSeries,
    [property: JsonPropertyName("novelDraft")] object[]? NovelDraft,
    [property: JsonPropertyName("collection")] object[]? Collection);
    
/// <summary>
/// 
/// </summary>
/// <param name="Illusts"></param>
/// <param name="Novels"></param>
public sealed record ThumbnailsMangaHomePage(
    [property: JsonPropertyName("illust")]
    List<MangaProfile> Illusts,

    [property: JsonPropertyName("novel")]
    List<NovelProfileHomePage> Novels);