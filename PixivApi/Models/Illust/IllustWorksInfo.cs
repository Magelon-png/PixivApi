namespace Scighost.PixivApi.Models.Illust;

/// <summary>
/// 
/// </summary>
public class IllustWorksInfo
{
    /// <summary>
    /// Illustration Id
    /// </summary>
    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int Id { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    /// <summary>
    /// 作品类型，插画 or 漫画
    /// </summary>
    [JsonPropertyName("illustType")]
    public IllustType IllustType { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    /// <summary>
    /// 介绍，html格式
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    /// <summary>
    /// 作品标签
    /// </summary>
    [JsonPropertyName("tags")]
    public string[] Tags { get; set; }
    
    /// <summary>
    /// 作者uid
    /// </summary>
    [JsonPropertyName("userId")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int UserId { get; set; }
    
    /// <summary>
    /// 作者昵称
    /// </summary>
    [JsonPropertyName("userName")]
    public string UserName { get; set; }
    
    /// <summary>
    /// 第一张图片的像素宽度
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; set; }

    /// <summary>
    /// 第一张图片的像素高度
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    /// <summary>
    /// 作品有多少张图片
    /// </summary>
    [JsonPropertyName("pageCount")]
    public int PageCount { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonPropertyName("createDate")]
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// 上传时间
    /// </summary>
    [JsonPropertyName("uploadDate")]
    public DateTimeOffset UploadDate { get; set; }
}