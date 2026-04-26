using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Illust;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// 
/// </summary>
/// <param name="TagTranslation"></param>
/// <param name="Thumbnails"></param>
/// <param name="IllustSeries"></param>
/// <param name="Requests"></param>
/// <param name="Users"></param>
/// <param name="Page"></param>
/// <param name="BoothItems"></param>
/// <param name="SketchLives"></param>
/// <param name="ZoneConfig"></param>
public record NovelHomePageResponse(
    [property: JsonPropertyName("tagTranslation")] Dictionary<string, TopIllustTagTranslation> TagTranslation,
    [property: JsonPropertyName("thumbnails")] ThumbnailsIllustHomePage Thumbnails,
    [property: JsonPropertyName("illustSeries")] object[] IllustSeries,
    [property: JsonPropertyName("requests")] Requests[] Requests,
    [property: JsonPropertyName("users")] Users[] Users,
    [property: JsonPropertyName("page")] NovelPage Page,
    [property: JsonPropertyName("boothItems")] BoothItems[] BoothItems,
    [property: JsonPropertyName("sketchLives")] object[] SketchLives,
    [property: JsonPropertyName("zoneConfig")] ZoneConfig ZoneConfig
);

//Recommend omited due to the api returning a JSON inside a string.
/// <summary>
/// 
/// </summary>
/// <param name="Tags"></param>
/// <param name="Follow"></param>
/// <param name="Mypixiv"></param>
/// <param name="RecommendByTag"></param>
/// <param name="Ranking"></param>
/// <param name="Pixivision"></param>
/// <param name="RecommendUser"></param>
/// <param name="ContestOngoing"></param>
/// <param name="ContestResult"></param>
/// <param name="EditorRecommend"></param>
/// <param name="BoothFollowItemIds"></param>
/// <param name="SketchLiveFollowIds"></param>
/// <param name="SketchLivePopularIds"></param>
/// <param name="MyFavoriteTags"></param>
/// <param name="NewPost"></param>
/// <param name="TrendingTags"></param>
/// <param name="CompleteRequestIds"></param>
/// <param name="UserEventIds"></param>
public record NovelPage(
    [property: JsonPropertyName("tags")] NovelTags[] Tags,
    [property: JsonPropertyName("follow")] 
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int[] Follow,
    [property: JsonPropertyName("mypixiv")] object[] Mypixiv,
    [property: JsonPropertyName("recommendByTag")] RecommendByTag[] RecommendByTag,
    [property: JsonPropertyName("ranking")] Ranking Ranking,
    [property: JsonPropertyName("pixivision")] Pixivision[] Pixivision,
    [property: JsonPropertyName("recommendUser")] TopIllustRecommendUser[] RecommendUser,
    [property: JsonPropertyName("contestOngoing")] ContestOngoing[] ContestOngoing,
    [property: JsonPropertyName("contestResult")] object[] ContestResult,
    [property: JsonPropertyName("editorRecommend")] EditorRecommend[] EditorRecommend,
    [property: JsonPropertyName("boothFollowItemIds")] string[] BoothFollowItemIds,
    [property: JsonPropertyName("sketchLiveFollowIds")] object[] SketchLiveFollowIds,
    [property: JsonPropertyName("sketchLivePopularIds")] object[] SketchLivePopularIds,
    [property: JsonPropertyName("myFavoriteTags")] string[] MyFavoriteTags,
    [property: JsonPropertyName("newPost")] string[] NewPost,
    [property: JsonPropertyName("trendingTags")] TrendingTags[] TrendingTags,
    [property: JsonPropertyName("completeRequestIds")] string[] CompleteRequestIds,
    [property: JsonPropertyName("userEventIds")] string[] UserEventIds
);

/// <summary>
/// 
/// </summary>
/// <param name="Tag"></param>
/// <param name="Ids"></param>
public record NovelTags(
    [property: JsonPropertyName("tag")] string Tag,
    [property: JsonPropertyName("ids")] 
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int[] Ids
);