using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.Illust;

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
public record IllustHomePageResponse(
    [property: JsonPropertyName("tagTranslation")] Dictionary<string, TopIllustTagTranslation> TagTranslation,
    [property: JsonPropertyName("thumbnails")] ThumbnailsIllustHomePage Thumbnails,
    [property: JsonPropertyName("illustSeries")] object[] IllustSeries,
    [property: JsonPropertyName("requests")] Requests[] Requests,
    [property: JsonPropertyName("users")] Users[] Users,
    [property: JsonPropertyName("page")] Page Page,
    [property: JsonPropertyName("boothItems")] BoothItems[] BoothItems,
    [property: JsonPropertyName("sketchLives")] object[] SketchLives,
    [property: JsonPropertyName("zoneConfig")] ZoneConfig ZoneConfig
);

/// <summary>
/// 
/// </summary>
/// <param name="English"></param>
/// <param name="Korean"></param>
/// <param name="Chinese"></param>
/// <param name="TraditionalChinese"></param>
/// <param name="Japanese"></param>
public record TopIllustTagTranslation(
    [property: JsonPropertyName("English")] string English,
    [property: JsonPropertyName("Korean")] string Korean,
    [property: JsonPropertyName("Chinese")] string Chinese,
    [property: JsonPropertyName("TraditionalChinese")] string TraditionalChinese,
    [property: JsonPropertyName("Japanese")] string Japanese
);

/// <summary>
/// 
/// </summary>
/// <param name="WorkTitle"></param>
/// <param name="WorkCaption"></param>
public record TitleCaptionTranslation(
    [property: JsonPropertyName("workTitle")] string WorkTitle,
    [property: JsonPropertyName("workCaption")] string WorkCaption
);

/// <summary>
/// 
/// </summary>
/// <param name="ExtraSmall"></param>
/// <param name="Small"></param>
/// <param name="Medium"></param>
/// <param name="Large"></param>
public record Urls(
    [property: JsonPropertyName("50x250")] string ExtraSmall,
    [property: JsonPropertyName("60x360")] string Small,
    [property: JsonPropertyName("40x540")] string Medium,
    [property: JsonPropertyName("200x1200")] string Large
);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Genre"></param>
/// <param name="XRestrict"></param>
/// <param name="Restrict"></param>
/// <param name="Url"></param>
/// <param name="Tags"></param>
/// <param name="UserId"></param>
/// <param name="UserName"></param>
/// <param name="ProfileImageUrl"></param>
/// <param name="TextCount"></param>
/// <param name="WordCount"></param>
/// <param name="ReadingTime"></param>
/// <param name="UseWordCount"></param>
/// <param name="Description"></param>
/// <param name="IsBookmarkable"></param>
/// <param name="BookmarkData"></param>
/// <param name="BookmarkCount"></param>
/// <param name="IsOriginal"></param>
/// <param name="Marker"></param>
/// <param name="TitleCaptionTranslation"></param>
/// <param name="CreateDate"></param>
/// <param name="UpdateDate"></param>
/// <param name="IsMasked"></param>
/// <param name="AiType"></param>
/// <param name="SeriesId"></param>
/// <param name="SeriesTitle"></param>
/// <param name="SeriesContentOrder"></param>
/// <param name="IsUnlisted"></param>
/// <param name="VisibilityScope"></param>
/// <param name="Language"></param>
public record Novel(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("genre")] string Genre,
    [property: JsonPropertyName("xRestrict")] int XRestrict,
    [property: JsonPropertyName("restrict")] int Restrict,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("tags")] string[] Tags,
    [property: JsonPropertyName("userId")] string UserId,
    [property: JsonPropertyName("userName")] string UserName,
    [property: JsonPropertyName("profileImageUrl")] string ProfileImageUrl,
    [property: JsonPropertyName("textCount")] int TextCount,
    [property: JsonPropertyName("wordCount")] int WordCount,
    [property: JsonPropertyName("readingTime")] int ReadingTime,
    [property: JsonPropertyName("useWordCount")] bool UseWordCount,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("isBookmarkable")] bool IsBookmarkable,
    [property: JsonPropertyName("bookmarkData")] object BookmarkData,
    [property: JsonPropertyName("bookmarkCount")] int BookmarkCount,
    [property: JsonPropertyName("isOriginal")] bool IsOriginal,
    [property: JsonPropertyName("marker")] object Marker,
    [property: JsonPropertyName("titleCaptionTranslation")] TitleCaptionTranslation1 TitleCaptionTranslation,
    [property: JsonPropertyName("createDate")] string CreateDate,
    [property: JsonPropertyName("updateDate")] string UpdateDate,
    [property: JsonPropertyName("isMasked")] bool IsMasked,
    [property: JsonPropertyName("aiType")] int AiType,
    [property: JsonPropertyName("seriesId")] string SeriesId,
    [property: JsonPropertyName("seriesTitle")] string SeriesTitle,
    [property: JsonPropertyName("seriesContentOrder")] int SeriesContentOrder,
    [property: JsonPropertyName("isUnlisted")] bool IsUnlisted,
    [property: JsonPropertyName("visibilityScope")] int VisibilityScope,
    [property: JsonPropertyName("language")] string Language
);

/// <summary>
/// 
/// </summary>
/// <param name="WorkTitle"></param>
/// <param name="WorkCaption"></param>
public record TitleCaptionTranslation1(
    [property: JsonPropertyName("workTitle")] object WorkTitle,
    [property: JsonPropertyName("workCaption")] object WorkCaption
);

/// <summary>
/// 
/// </summary>
/// <param name="RequestId"></param>
/// <param name="PlanId"></param>
/// <param name="FanUserId"></param>
/// <param name="CreatorUserId"></param>
/// <param name="RequestStatus"></param>
/// <param name="RequestPostWorkType"></param>
/// <param name="RequestPrice"></param>
/// <param name="RequestProposal"></param>
/// <param name="RequestTags"></param>
/// <param name="RequestAdultFlg"></param>
/// <param name="RequestAnonymousFlg"></param>
/// <param name="RequestRestrictFlg"></param>
/// <param name="RequestAcceptCollaborateFlg"></param>
/// <param name="RequestResponseDeadlineDatetime"></param>
/// <param name="RequestPostDeadlineDatetime"></param>
/// <param name="Role"></param>
/// <param name="Plan"></param>
/// <param name="CollaborateStatus"></param>
/// <param name="GiftFile"></param>
/// <param name="PostWork"></param>
/// <param name="NotifyBadge"></param>
/// <param name="Fanbox"></param>
/// <param name="RequestResend"></param>
public record Requests(
    [property: JsonPropertyName("requestId")] string RequestId,
    [property: JsonPropertyName("planId")] string PlanId,
    [property: JsonPropertyName("fanUserId")] string FanUserId,
    [property: JsonPropertyName("creatorUserId")] string CreatorUserId,
    [property: JsonPropertyName("requestStatus")] string RequestStatus,
    [property: JsonPropertyName("requestPostWorkType")] string RequestPostWorkType,
    [property: JsonPropertyName("requestPrice")] int RequestPrice,
    [property: JsonPropertyName("requestProposal")] RequestProposal RequestProposal,
    [property: JsonPropertyName("requestTags")] string[] RequestTags,
    [property: JsonPropertyName("requestAdultFlg")] bool RequestAdultFlg,
    [property: JsonPropertyName("requestAnonymousFlg")] bool RequestAnonymousFlg,
    [property: JsonPropertyName("requestRestrictFlg")] bool RequestRestrictFlg,
    [property: JsonPropertyName("requestAcceptCollaborateFlg")] bool RequestAcceptCollaborateFlg,
    [property: JsonPropertyName("requestResponseDeadlineDatetime")] string RequestResponseDeadlineDatetime,
    [property: JsonPropertyName("requestPostDeadlineDatetime")] string RequestPostDeadlineDatetime,
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("plan")] Plan Plan,
    [property: JsonPropertyName("collaborateStatus")] CollaborateStatus CollaborateStatus,
    [property: JsonPropertyName("giftFile")] object GiftFile,
    [property: JsonPropertyName("postWork")] PostWork PostWork,
    [property: JsonPropertyName("notifyBadge")] object NotifyBadge,
    [property: JsonPropertyName("fanbox")] object Fanbox,
    [property: JsonPropertyName("requestResend")] RequestResend RequestResend
);

/// <summary>
/// 
/// </summary>
/// <param name="RequestOriginalProposal"></param>
/// <param name="RequestOriginalProposalHtml"></param>
/// <param name="RequestOriginalProposalLang"></param>
/// <param name="RequestTranslationProposal"></param>
public record RequestProposal(
    [property: JsonPropertyName("requestOriginalProposal")] string RequestOriginalProposal,
    [property: JsonPropertyName("requestOriginalProposalHtml")] string RequestOriginalProposalHtml,
    [property: JsonPropertyName("requestOriginalProposalLang")] string RequestOriginalProposalLang,
    [property: JsonPropertyName("requestTranslationProposal")] RequestTranslationProposal[] RequestTranslationProposal
);

/// <summary>
/// 
/// </summary>
/// <param name="RequestProposal"></param>
/// <param name="RequestProposalHtml"></param>
/// <param name="RequestProposalLang"></param>
public record RequestTranslationProposal(
    [property: JsonPropertyName("requestProposal")] string RequestProposal,
    [property: JsonPropertyName("requestProposalHtml")] string RequestProposalHtml,
    [property: JsonPropertyName("requestProposalLang")] string RequestProposalLang
);

/// <summary>
/// 
/// </summary>
/// <param name="CurrentPlanId"></param>
/// <param name="PlanId"></param>
/// <param name="CreatorUserId"></param>
/// <param name="PlanAcceptRequestFlg"></param>
/// <param name="PlanStandardPrice"></param>
/// <param name="PlanTitle"></param>
/// <param name="PlanDescription"></param>
/// <param name="PlanAcceptAdultFlg"></param>
/// <param name="PlanAcceptAnonymousFlg"></param>
/// <param name="PlanAcceptIllustFlg"></param>
/// <param name="PlanAcceptUgoiraFlg"></param>
/// <param name="PlanAcceptMangaFlg"></param>
/// <param name="PlanAcceptNovelFlg"></param>
/// <param name="PlanCoverImage"></param>
/// <param name="PlanAiType"></param>
public record Plan(
    [property: JsonPropertyName("currentPlanId")] object CurrentPlanId,
    [property: JsonPropertyName("planId")] string PlanId,
    [property: JsonPropertyName("creatorUserId")] string CreatorUserId,
    [property: JsonPropertyName("planAcceptRequestFlg")] bool PlanAcceptRequestFlg,
    [property: JsonPropertyName("planStandardPrice")] int PlanStandardPrice,
    [property: JsonPropertyName("planTitle")] PlanTitle PlanTitle,
    [property: JsonPropertyName("planDescription")] PlanDescription PlanDescription,
    [property: JsonPropertyName("planAcceptAdultFlg")] bool PlanAcceptAdultFlg,
    [property: JsonPropertyName("planAcceptAnonymousFlg")] bool PlanAcceptAnonymousFlg,
    [property: JsonPropertyName("planAcceptIllustFlg")] bool PlanAcceptIllustFlg,
    [property: JsonPropertyName("planAcceptUgoiraFlg")] bool PlanAcceptUgoiraFlg,
    [property: JsonPropertyName("planAcceptMangaFlg")] bool PlanAcceptMangaFlg,
    [property: JsonPropertyName("planAcceptNovelFlg")] bool PlanAcceptNovelFlg,
    [property: JsonPropertyName("planCoverImage")] PlanCoverImage PlanCoverImage,
    [property: JsonPropertyName("planAiType")] int PlanAiType
);

/// <summary>
/// 
/// </summary>
/// <param name="PlanOriginalTitle"></param>
/// <param name="PlanOriginalTitleLang"></param>
/// <param name="PlanTranslationTitle"></param>
public record PlanTitle(
    [property: JsonPropertyName("planOriginalTitle")] string PlanOriginalTitle,
    [property: JsonPropertyName("planOriginalTitleLang")] string PlanOriginalTitleLang,
    [property: JsonPropertyName("planTranslationTitle")] 
    [property: JsonConverter(typeof(EmptyArrayAsDictionaryJsonConverter<PlanTranslationTitle>))]
    Dictionary<string, PlanTranslationTitle> PlanTranslationTitle
);


/// <summary>
/// 
/// </summary>
/// <param name="PlanTitle"></param>
/// <param name="PlanTitleLang"></param>
public record PlanTranslationTitle(
    [property: JsonPropertyName("planTitle")] string PlanTitle,
    [property: JsonPropertyName("planTitleLang")] string PlanTitleLang
);

/// <summary>
/// 
/// </summary>
/// <param name="PlanOriginalDescription"></param>
/// <param name="PlanOriginalDescriptionHtml"></param>
/// <param name="PlanOriginalLang"></param>
/// <param name="PlanTranslationDescription"></param>
public record PlanDescription(
    [property: JsonPropertyName("planOriginalDescription")] string PlanOriginalDescription,
    [property: JsonPropertyName("planOriginalDescriptionHtml")] string PlanOriginalDescriptionHtml,
    [property: JsonPropertyName("planOriginalLang")] string PlanOriginalLang,
    [property: JsonPropertyName("planTranslationDescription")] 
    [property: JsonConverter(typeof(EmptyArrayAsDictionaryJsonConverter<PlanTranslationDescription>))]
    Dictionary<string, PlanTranslationDescription> PlanTranslationDescription
);

/// <summary>
/// 
/// </summary>
/// <param name="PlanDescription"></param>
/// <param name="PlanDescriptionHtml"></param>
/// <param name="PlanLang"></param>
public record PlanTranslationDescription(
    [property: JsonPropertyName("planDescription")] string PlanDescription,
    [property: JsonPropertyName("planDescriptionHtml")] string PlanDescriptionHtml,
    [property: JsonPropertyName("planLang")] string PlanLang
);

/// <summary>
/// 
/// </summary>
/// <param name="Urls"></param>
public record PlanCoverImage(
    [property: JsonPropertyName("urls")] Urls1 Urls
);

/// <summary>
/// 
/// </summary>
/// <param name="Cover"></param>
/// <param name="Card"></param>
public record Urls1(
    [property: JsonPropertyName("cover")] string Cover,
    [property: JsonPropertyName("card")] string Card
);

/// <summary>
/// 
/// </summary>
/// <param name="Collaborating"></param>
/// <param name="CollaborateAnonymousFlg"></param>
/// <param name="CollaboratedCnt"></param>
/// <param name="CollaborateUserSamples"></param>
public record CollaborateStatus(
    [property: JsonPropertyName("collaborating")] bool Collaborating,
    [property: JsonPropertyName("collaborateAnonymousFlg")] bool CollaborateAnonymousFlg,
    [property: JsonPropertyName("collaboratedCnt")] int CollaboratedCnt,
    [property: JsonPropertyName("collaborateUserSamples")] object[] CollaborateUserSamples
);

/// <summary>
/// 
/// </summary>
/// <param name="PostWorkId"></param>
/// <param name="PostWorkType"></param>
/// <param name="Work"></param>
public record PostWork(
    [property: JsonPropertyName("postWorkId")] string PostWorkId,
    [property: JsonPropertyName("postWorkType")] string PostWorkType,
    [property: JsonPropertyName("work")] Work Work
);

/// <summary>
/// 
/// </summary>
/// <param name="IsUnlisted"></param>
/// <param name="Secret"></param>
public record Work(
    [property: JsonPropertyName("isUnlisted")] bool IsUnlisted,
    [property: JsonPropertyName("secret")] object Secret
);

/// <summary>
/// 
/// </summary>
/// <param name="RequestResendDeadlineDatetime"></param>
/// <param name="RequestResendOfferEnabled"></param>
/// <param name="RequestResendEnabled"></param>
/// <param name="RequestResendStatus"></param>
/// <param name="Modification"></param>
/// <param name="FanAdultSendable"></param>
/// <param name="IsResentRequest"></param>
public record RequestResend(
    [property: JsonPropertyName("requestResendDeadlineDatetime")] object RequestResendDeadlineDatetime,
    [property: JsonPropertyName("requestResendOfferEnabled")] object RequestResendOfferEnabled,
    [property: JsonPropertyName("requestResendEnabled")] object RequestResendEnabled,
    [property: JsonPropertyName("requestResendStatus")] object RequestResendStatus,
    [property: JsonPropertyName("modification")] Modification Modification,
    [property: JsonPropertyName("fanAdultSendable")] object FanAdultSendable,
    [property: JsonPropertyName("isResentRequest")] object IsResentRequest
);

/// <summary>
/// 
/// </summary>
/// <param name="RequestPostWorkType"></param>
/// <param name="RequestAdultFlg"></param>
public record Modification(
    [property: JsonPropertyName("requestPostWorkType")] object RequestPostWorkType,
    [property: JsonPropertyName("requestAdultFlg")] object RequestAdultFlg
);

/// <summary>
/// 
/// </summary>
/// <param name="Partial"></param>
/// <param name="Comment"></param>
/// <param name="FollowedBack"></param>
/// <param name="UserId"></param>
/// <param name="Name"></param>
/// <param name="Image"></param>
/// <param name="ImageBig"></param>
/// <param name="Premium"></param>
/// <param name="IsFollowed"></param>
/// <param name="IsMypixiv"></param>
/// <param name="IsBlocking"></param>
/// <param name="Background"></param>
/// <param name="Commission"></param>
public record Users(
    [property: JsonPropertyName("partial")] int Partial,
    [property: JsonPropertyName("comment")] string Comment,
    [property: JsonPropertyName("followedBack")] bool FollowedBack,
    [property: JsonPropertyName("userId")] string UserId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("image")] string Image,
    [property: JsonPropertyName("imageBig")] string ImageBig,
    [property: JsonPropertyName("premium")] bool Premium,
    [property: JsonPropertyName("isFollowed")] bool IsFollowed,
    [property: JsonPropertyName("isMypixiv")] bool IsMypixiv,
    [property: JsonPropertyName("isBlocking")] bool IsBlocking,
    [property: JsonPropertyName("background")] object Background,
    [property: JsonPropertyName("commission")] Commission Commission
);

/// <summary>
/// 
/// </summary>
/// <param name="AcceptRequest"></param>
/// <param name="IsSubscribedReopenNotification"></param>
public record Commission(
    [property: JsonPropertyName("acceptRequest")] bool AcceptRequest,
    [property: JsonPropertyName("isSubscribedReopenNotification")] bool IsSubscribedReopenNotification
);

/// <summary>
/// 
/// </summary>
/// <param name="Tags"></param>
/// <param name="Follow"></param>
/// <param name="Mypixiv"></param>
/// <param name="Recommend"></param>
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
public record Page(
    [property: JsonPropertyName("tags")] IllustMangaTags[] Tags,
    [property: JsonPropertyName("follow")] 
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int[] Follow,
    [property: JsonPropertyName("mypixiv")] object[] Mypixiv,
    [property: JsonPropertyName("recommend")] Recommend Recommend,
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
public record IllustMangaTags(
    [property: JsonPropertyName("tag")] string Tag,
    [property: JsonPropertyName("ids")] int[] Ids
);

/// <summary>
/// 
/// </summary>
/// <param name="Ids"></param>
/// <param name="Details"></param>
public record Recommend(
    [property: JsonPropertyName("ids")] string[] Ids,
    [property: JsonPropertyName("details")] Dictionary<string, Details> Details
);

/// <summary>
/// 
/// </summary>
/// <param name="Methods"></param>
/// <param name="Score"></param>
/// <param name="SeedIllustIds"></param>
public record Details(
    [property: JsonPropertyName("methods")] string[] Methods,
    [property: JsonPropertyName("score")] double Score,
    [property: JsonPropertyName("seedIllustIds")] string[] SeedIllustIds
);

/// <summary>
/// 
/// </summary>
/// <param name="Tag"></param>
/// <param name="Ids"></param>
/// <param name="Details"></param>
public record RecommendByTag(
    [property: JsonPropertyName("tag")] string Tag,
    [property: JsonPropertyName("ids")] string[] Ids,
    [property: JsonPropertyName("details")] Dictionary<string, Details> Details
);

/// <summary>
/// 
/// </summary>
/// <param name="Items"></param>
/// <param name="Date"></param>
public record Ranking(
    [property: JsonPropertyName("items")] Items[] Items,
    [property: JsonPropertyName("date")] string Date
);

/// <summary>
/// 
/// </summary>
/// <param name="Rank"></param>
/// <param name="Id"></param>
public record Items(
    [property: JsonPropertyName("rank")] string Rank,
    [property: JsonPropertyName("id")] string Id
);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="ThumbnailUrl"></param>
/// <param name="Url"></param>
public record Pixivision(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("thumbnailUrl")] string ThumbnailUrl,
    [property: JsonPropertyName("url")] string Url
);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="IllustIds"></param>
/// <param name="NovelIds"></param>
public record TopIllustRecommendUser(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("illustIds")] string[] IllustIds,
    [property: JsonPropertyName("novelIds")] object[] NovelIds
);

/// <summary>
/// 
/// </summary>
/// <param name="Slug"></param>
/// <param name="Type"></param>
/// <param name="Name"></param>
/// <param name="Url"></param>
/// <param name="IconUrl"></param>
/// <param name="WorkIds"></param>
/// <param name="IsNew"></param>
public record ContestOngoing(
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("iconUrl")] string IconUrl,
    [property: JsonPropertyName("workIds")] int[] WorkIds,
    [property: JsonPropertyName("isNew")] bool IsNew
);

/// <summary>
/// 
/// </summary>
/// <param name="IllustId"></param>
/// <param name="Comment"></param>
public record EditorRecommend(
    [property: JsonPropertyName("illustId")] string IllustId,
    [property: JsonPropertyName("comment")] string Comment
);

/// <summary>
/// 
/// </summary>
/// <param name="Tag"></param>
/// <param name="TrendingRate"></param>
/// <param name="Ids"></param>
public record TrendingTags(
    [property: JsonPropertyName("tag")] string Tag,
    [property: JsonPropertyName("trendingRate")] int TrendingRate,
    [property: JsonPropertyName("ids")] int[] Ids
);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="UserId"></param>
/// <param name="Title"></param>
/// <param name="Url"></param>
/// <param name="ImageUrl"></param>
/// <param name="Adult"></param>
public record BoothItems(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("userId")] string UserId,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("imageUrl")] string ImageUrl,
    [property: JsonPropertyName("adult")] bool Adult
);

/// <summary>
/// 
/// </summary>
/// <param name="Header"></param>
/// <param name="Footer"></param>
/// <param name="TopbrandingRectangle"></param>
/// <param name="Comic"></param>
/// <param name="IllusttopAppeal"></param>
/// <param name="Logo"></param>
/// <param name="AdLogo"></param>
public record ZoneConfig(
    [property: JsonPropertyName("header")] ZoneData Header,
    [property: JsonPropertyName("footer")] ZoneData Footer,
    [property: JsonPropertyName("topbranding_rectangle")] ZoneData TopbrandingRectangle,
    [property: JsonPropertyName("comic")] ZoneData Comic,
    [property: JsonPropertyName("illusttop_appeal")] ZoneData IllusttopAppeal,
    [property: JsonPropertyName("logo")] ZoneData Logo,
    [property: JsonPropertyName("ad_logo")] ZoneData AdLogo
);

/// <summary>
/// 
/// </summary>
/// <param name="Url"></param>
public record ZoneData(
    [property: JsonPropertyName("url")] string Url
);
