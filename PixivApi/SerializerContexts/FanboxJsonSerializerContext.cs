using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Fanbox;

namespace Scighost.PixivApi.SerializerContexts;


[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(FanboxResponseWrapper<string[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<SupportingPlan[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<FollowedCreator[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<PostListItem[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<PostInfo>))]
public  partial class FanboxJsonSerializerContext : JsonSerializerContext
{
    
}