namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// Result of notification list
/// </summary>
/// <param name="items">List of notifications</param>
/// <param name="nextUrl">URL for next page</param>
public record GetNotificationResult(
    [property: JsonPropertyName("items")] NotificationContent[] items,
    [property: JsonPropertyName("nextUrl")] string? nextUrl);

/// <summary>
/// Notification content
/// </summary>
/// <param name="Id">Notification ID</param>
/// <param name="NotifiedDatetime">When the notification was sent</param>
/// <param name="Type">Type of notification</param>
/// <param name="Post">Related post</param>
/// <param name="IsUnread">Whether the notification is unread</param>
public record NotificationContent(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("notifiedDatetime")]
    DateTimeOffset NotifiedDatetime,
    [property: JsonPropertyName("type")]
    //This is an enum but I do not know all values
    string Type,
    [property: JsonPropertyName("post")]
    PostListItem Post,
    [property: JsonPropertyName("isUnread")]
    bool IsUnread
);