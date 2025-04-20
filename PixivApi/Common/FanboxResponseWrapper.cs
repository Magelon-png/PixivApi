namespace Scighost.PixivApi.Common;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class FanboxResponseWrapper<T>
{
    [JsonPropertyName("body")]
    public T Body { get; set; }
}