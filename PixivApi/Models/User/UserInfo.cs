using Scighost.PixivApi.Models.Common;

namespace Scighost.PixivApi.Models.User;


/// <summary>
/// 用户信息
/// </summary>
public class UserInfo : UserProfile
{


    /// <summary>
    /// 用户个人背景，此值为 null ，api 已不再能获取到背景图
    /// </summary>
    [JsonPropertyName("background")]
    public UserBackground? Background { get; set; }

    /// <summary>
    /// 该用户的关注用户数
    /// </summary>
    [JsonPropertyName("following")]
    public int FollowingCount { get; set; }

    /// <summary>
    /// 个人介绍，html格式
    /// </summary>
    [JsonPropertyName("commentHtml")]
    public string CommentHtml { get; set; }

    /// <summary>
    /// 不懂
    /// </summary>
    [JsonPropertyName("webpage")]
    public string Webpage { get; set; }

    /// <summary>
    /// 不懂
    /// </summary>
    [JsonPropertyName("official")]
    public bool Official { get; set; }

    /// <summary>
    /// 群组
    /// </summary>
    [JsonPropertyName("group")]
    public object Group { get; set; }
    
    /// <summary>
    /// If the provided profile is containing partial information
    /// </summary>
    [JsonPropertyName("partial")]
    [JsonConverter(typeof(BoolToNumberJsonConverter))]
    public bool Partial { get; set; }
}


/// <summary>
/// 用户个人背景图
/// </summary>
public class UserBackground
{
    /// <summary>
    /// 不懂
    /// </summary>
    [JsonPropertyName("repeat")]
    public object Repeat { get; set; }

    /// <summary>
    /// 不懂
    /// </summary>
    [JsonPropertyName("color")]
    public object Color { get; set; }

    /// <summary>
    /// 图片，可能没有
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// 不懂
    /// </summary>
    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; set; }
}