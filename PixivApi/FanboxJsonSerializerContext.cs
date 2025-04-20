using Scighost.PixivApi.Common;
using Scighost.PixivApi.Fanbox;

namespace Scighost.PixivApi;


[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(FanboxResponseWrapper<string[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<SupportingPlan[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<FollowedCreator[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<PostListItem[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<PostInfo>))]
public  partial class FanboxJsonSerializerContext : JsonSerializerContext
{
    
}