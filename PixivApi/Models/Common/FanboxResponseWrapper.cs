namespace Scighost.PixivApi.Models.Common;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Body"></param>
public record FanboxResponseWrapper<T>(
    [property: JsonPropertyName("body")]
    T Body
);