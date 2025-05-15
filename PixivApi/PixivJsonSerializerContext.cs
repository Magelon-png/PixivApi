using System.Text.Json;
using System.Text.Json.Nodes;
using Scighost.PixivApi.Common;
using Scighost.PixivApi.Fanbox;
using Scighost.PixivApi.Search;
using Scighost.PixivApi.V2;
using Scighost.PixivApi.V2.Illust;
using IllustInfo = Scighost.PixivApi.Illust.IllustInfo;
using IllustInfoResponse = Scighost.PixivApi.V2.Illust.IllustInfoResponse;

namespace Scighost.PixivApi;


[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(Dictionary<int, object>))]
[JsonSerializable(typeof(Dictionary<int, IllustProfile>))]
[JsonSerializable(typeof(PixivTagInternal))]
[JsonSerializable(typeof(PixivResponseWrapper<List<SearchCandidate>>))]
[JsonSerializable(typeof(PixivResponseWrapper<string>))]
[JsonSerializable(typeof(PixivResponseWrapper<JsonNode>))]
[JsonSerializable(typeof(PixivResponseWrapper<UserInfo>))]
[JsonSerializable(typeof(PixivResponseWrapper<UserTopWorks>))]
[JsonSerializable(typeof(PixivResponseWrapper<UserAllWorks>))]
[JsonSerializable(typeof(PixivResponseWrapper<IllustInfo>))]
[JsonSerializable(typeof(PixivResponseWrapper<IllustWorks>))]
[JsonSerializable(typeof(PixivResponseWrapper<UserIllustsByTag>))]
[JsonSerializable(typeof(PixivResponseWrapper<List<IllustImage>>))]
[JsonSerializable(typeof(PixivResponseWrapper<AnimateIllustMeta>))]
[JsonSerializable(typeof(PixivResponseWrapper<MangaSeriesResponse>))]
[JsonSerializable(typeof(PixivResponseWrapper<RecommendIllustWrapper>))]
[JsonSerializable(typeof(PixivResponseWrapper<NovelInfo>))]
[JsonSerializable(typeof(PixivResponseWrapper<NovelSeries>))]
[JsonSerializable(typeof(PixivResponseWrapper<NovelSeriesContentWrapper>))]
[JsonSerializable(typeof(PixivResponseWrapper<RecommendNovelWrapper>))]
[JsonSerializable(typeof(PixivResponseWrapper<BookmarkIllustWrapper>))]
[JsonSerializable(typeof(PixivResponseWrapper<BookmarkNovelWrapper>))]
[JsonSerializable(typeof(PixivResponseWrapper<UserBookmarkTag>))]
[JsonSerializable(typeof(PixivResponseWrapper<FollowingUserWrapper>))]
[JsonSerializable(typeof(PixivResponseWrapper<FollowingLatestWorkWrapper>))]
[JsonSerializable(typeof(PixivResponseWrapper<RecommendUserResponse>))]
[JsonSerializable(typeof(PixivResponseWrapper<IllustSearchResult>))]
internal partial class PixivJsonSerializerContext : JsonSerializerContext
{
}
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
internal partial class PixivV2JsonSerializerContext : JsonSerializerContext
{
}