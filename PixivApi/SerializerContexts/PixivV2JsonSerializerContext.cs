using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.V2.Common;
using Scighost.PixivApi.Models.V2.Illust;
using IllustInfoResponse = Scighost.PixivApi.Models.V2.Illust.IllustInfoResponse;

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
internal partial class PixivV2JsonSerializerContext : JsonSerializerContext
{
}