namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="FeeRequired"></param>
/// <param name="PublishedDatetime"></param>
/// <param name="UpdatedDatetime"></param>
/// <param name="Tags"></param>
/// <param name="IsLiked"></param>
/// <param name="Likes"></param>
/// <param name="IsCommentingRestricted"></param>
/// <param name="CommentCount"></param>
/// <param name="IsRestricted">When true, means that the post is locked from not being in a high enough tier for example</param>
/// <param name="User"></param>
/// <param name="CreatorId"></param>
/// <param name="HasAdultContent"></param>
/// <param name="Type"></param>
/// <param name="CoverImageUrl"></param>
/// <param name="Body"></param>
/// <param name="Excerpt"></param>
/// <param name="NextPost"></param>
/// <param name="PrevPost"></param>
/// <param name="ImageForShare"></param>
/// <param name="IsPinned"></param>
public record PostInfo(
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString),
    JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("title")]
    string Title,
    [property: JsonPropertyName("feeRequired")]
    int FeeRequired,
    [property: JsonPropertyName("publishedDatetime")]
    DateTimeOffset PublishedDatetime,
    [property: JsonPropertyName("updatedDatetime")]
    DateTimeOffset UpdatedDatetime,
    [property: JsonPropertyName("tags")]
    string[] Tags,
    [property: JsonPropertyName("isLiked")]
    bool IsLiked,
    [property: JsonPropertyName("likeCount")]
    int Likes,
    [property: JsonPropertyName("isCommentingRestricted")]
    bool IsCommentingRestricted,
    [property: JsonPropertyName("commentCount")]
    int CommentCount,
    [property: JsonPropertyName("isRestricted")]
    bool IsRestricted,
    [property: JsonPropertyName("user")]
    FanboxUser User,
    [property: JsonPropertyName("creatorId")]
    string CreatorId,
    [property: JsonPropertyName("hasAdultContent")]
    bool HasAdultContent,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("coverImageUrl")]
    string CoverImageUrl,
    [property: JsonPropertyName("body")]
    PostContent? Body,
    [property: JsonPropertyName("excerpt")]
    string Excerpt,
    [property: JsonPropertyName("nextPost")]
    AdjacentPostInfo NextPost,
    [property: JsonPropertyName("prevPost")]
    AdjacentPostInfo PrevPost,
    [property: JsonPropertyName("imageForShare")]
    string ImageForShare,
    [property: JsonPropertyName("isPinned")]
    bool IsPinned
);