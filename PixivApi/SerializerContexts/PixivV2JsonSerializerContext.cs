using Scighost.PixivApi.Common;
using Scighost.PixivApi.V2;
using Scighost.PixivApi.V2.Illust;
using IllustInfoResponse = Scighost.PixivApi.V2.Illust.IllustInfoResponse;

namespace Scighost.PixivApi;

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