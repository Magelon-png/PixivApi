using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.User;


internal record FollowingLatestWorkWrapper(
    [property: JsonPropertyName("thumbnails")]
    Thumbnails Thumbnails
);

