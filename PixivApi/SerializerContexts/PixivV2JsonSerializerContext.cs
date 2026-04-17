using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.V2.Common;
using Scighost.PixivApi.Models.V2.Illust;
using Scighost.PixivApi.Models.V2.Novel;
using Scighost.PixivApi.Models.V2.User;
using IllustInfoResponse = Scighost.PixivApi.Models.V2.Illust.IllustInfoResponse;
using NovelInfoResponse = Scighost.PixivApi.Models.V2.Novel.NovelInfoResponse;

namespace Scighost.PixivApi.SerializerContexts;

[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(PixivV2ErrorWrapper))]
[JsonSerializable(typeof(OauthLoginBody))]
[JsonSerializable(typeof(OauthLoginResponse))]
[JsonSerializable(typeof(IllustInfoResponse))]
[JsonSerializable(typeof(IllustsInfoResponse))]
[JsonSerializable(typeof(UgoiraMetadataResponse))]
[JsonSerializable(typeof(IllustCommentsResponse))]
[JsonSerializable(typeof(BookmarkDetailResponse))]
[JsonSerializable(typeof(RecommendedIllustResponse))]
[JsonSerializable(typeof(IllustSearchResponse))]
[JsonSerializable(typeof(NovelsInfoResponse))]
[JsonSerializable(typeof(NovelInfoResponse))]
[JsonSerializable(typeof(RecommendedNovelResponse))]
[JsonSerializable(typeof(UserFollowDetailResponse))]
[JsonSerializable(typeof(UserBookmarkTagsResponse))]
[JsonSerializable(typeof(TrendingTagsResponse))]
[JsonSerializable(typeof(WalkthroughResponse))]
internal partial class PixivV2JsonSerializerContext : JsonSerializerContext
{
}