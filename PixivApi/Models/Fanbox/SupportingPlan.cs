namespace Scighost.PixivApi.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="CoverImageUrl"></param>
/// <param name="CreatorId"></param>
/// <param name="User"></param>
/// <param name="HasAdultContent"></param>
/// <param name="PaymentMethod"></param>
public record SupportingPlan(
    [property: JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
    int Id,
    [property: JsonPropertyName("title")]
    string Title,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("coverImageUrl")]
    string CoverImageUrl,
    [property: JsonPropertyName("creatorId")]
    string CreatorId,
    [property: JsonPropertyName("user")]
    FanboxUser User,
    [property: JsonPropertyName("hasAdultContent")]
    bool HasAdultContent,
    [property: JsonPropertyName("paymentMethod")]
    string PaymentMethod);