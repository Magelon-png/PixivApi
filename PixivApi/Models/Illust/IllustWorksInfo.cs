namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
/// <param name="Id">Illustration ID</param>
/// <param name="Title"></param>
/// <param name="IllustType">Work type, illustration or manga</param>
/// <param name="Url"></param>
/// <param name="Description">Description, HTML format</param>
/// <param name="Tags">Work tags</param>
/// <param name="UserId">Author uid</param>
/// <param name="UserName">Author nickname</param>
/// <param name="Width">Pixel width of the first image</param>
/// <param name="Height">Pixel height of the first image</param>
/// <param name="PageCount">Number of images in the work</param>
/// <param name="CreateDate">Creation time</param>
/// <param name="UploadDate">Upload time</param>
/// <param name="AiType"></param>
public record IllustWorksInfo(
    [property: JsonPropertyName("id")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("illustType")]
    IllustType IllustType,

    [property: JsonPropertyName("url")]
    string Url,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("tags")]
    string[] Tags,

    [property: JsonPropertyName("userId")]
    [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int UserId,

    [property: JsonPropertyName("userName")]
    string UserName,

    [property: JsonPropertyName("width")]
    int Width,

    [property: JsonPropertyName("height")]
    int Height,

    [property: JsonPropertyName("pageCount")]
    int PageCount,

    [property: JsonPropertyName("createDate")]
    DateTimeOffset CreateDate,

    [property: JsonPropertyName("uploadDate")]
    DateTimeOffset UploadDate,

    [property: JsonPropertyName("aiType")]
    AiType AiType
);