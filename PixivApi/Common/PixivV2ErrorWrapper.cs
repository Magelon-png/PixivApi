namespace Scighost.PixivApi.Common;

/// <summary>
/// 
/// </summary>
/// <param name="Error"></param>
public record PixivV2ErrorWrapper([property: JsonPropertyName("error")] PixivV2ErrorBody Error);

/// <summary>
/// 
/// </summary>
/// <param name="UserMessage"></param>
/// <param name="Message"></param>
/// <param name="Reason"></param>
public record PixivV2ErrorBody([property: JsonPropertyName("user_message")]
string UserMessage,
[property: JsonPropertyName("message")]
string Message,
[property: JsonPropertyName("reason")]
string Reason);