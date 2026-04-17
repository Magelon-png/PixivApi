namespace Scighost.PixivApi.Models.Common;

internal record PixivResponseWrapper<T>(
    [property: JsonPropertyName("error")]
    bool Error,

    [property: JsonPropertyName("message")]
    string Message,

    [property: JsonPropertyName("body")]
    T Body
);


