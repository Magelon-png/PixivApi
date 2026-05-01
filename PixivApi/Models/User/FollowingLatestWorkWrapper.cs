using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.User;


internal sealed record FollowingLatestWorkWrapper(
    [property: JsonPropertyName("thumbnails")]
    Thumbnails Thumbnails
);

