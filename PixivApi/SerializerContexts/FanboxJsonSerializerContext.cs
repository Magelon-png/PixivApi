using Scighost.PixivApi.Clients;
using Scighost.PixivApi.Models.Common;
using Scighost.PixivApi.Models.Fanbox;

namespace Scighost.PixivApi.SerializerContexts;


/// <summary>
/// 
/// </summary>
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(FanboxResponseWrapper<string[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<SupportingPlan[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<FollowedCreator[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<PostListItem[]>))]
[JsonSerializable(typeof(FanboxResponseWrapper<PostInfo>))]
[JsonSerializable(typeof(FanboxResponseWrapper<CreatorSearchResult>))]
[JsonSerializable(typeof(FanboxResponseWrapper<SearchRecommendCreatorsResult>))]
[JsonSerializable(typeof(FanboxResponseWrapper<HomePagePostItems>))]
[JsonSerializable(typeof(FanboxResponseWrapper<EmptyResponse>))]
[JsonSerializable(typeof(FanboxResponseWrapper<GetNotificationResult>))]
public partial class FanboxJsonSerializerContext : JsonSerializerContext
{
    
}