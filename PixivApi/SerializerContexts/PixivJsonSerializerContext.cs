using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;
using Scighost.PixivApi.Models.Search;
using Scighost.PixivApi.Models.User;
using IllustInfo = Scighost.PixivApi.Models.Illust.IllustInfo;

namespace Scighost.PixivApi.SerializerContexts;


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