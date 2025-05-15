using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.User;


internal class FollowingLatestWorkWrapper
{

    [JsonPropertyName("thumbnails")]
    public Thumbnails Thumbnails { get; set; }
}

