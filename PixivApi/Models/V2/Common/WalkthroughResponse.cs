using System.Text.Json.Serialization;

namespace Scighost.PixivApi.Models.V2.Common;

/// <summary>
/// 
/// </summary>
/// <param name="Walkthroughs"></param>
public record WalkthroughResponse(
    [property: JsonPropertyName("walkthroughs")]
    List<Walkthrough> Walkthroughs);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public record Walkthrough(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("name")]
    string Name);
