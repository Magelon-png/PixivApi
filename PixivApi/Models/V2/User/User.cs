namespace Scighost.PixivApi.Models.V2.User;

/// <summary>
/// 
/// </summary>
/// <param name="ProfileImageUrls"></param>
/// <param name="Id"></param>
/// <param name="Name">Username</param>
/// <param name="Account">Account real name</param>
/// <param name="MailAddress"></param>
/// <param name="IsPremium"></param>
/// <param name="XRestrict"></param>
/// <param name="IsMailAuthorized"></param>
public record User(
    [property: JsonPropertyName("profile_image_urls")]
    ProfileImageUrls ProfileImageUrls,
    [property: JsonPropertyName("id"), 
               JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    uint Id,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("account")]
    string Account,
    [property: JsonPropertyName("mail_address")]
    string MailAddress,
    [property: JsonPropertyName("is_premium")]
    bool IsPremium,
    [property: JsonPropertyName("x_restrict")]
    int XRestrict,
    [property: JsonPropertyName("is_mail_authorized")]
    bool IsMailAuthorized
);