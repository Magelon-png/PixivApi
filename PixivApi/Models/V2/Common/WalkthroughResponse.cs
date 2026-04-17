using System.Text.Json.Serialization;

namespace Scighost.PixivApi.Models.V2.Common;

public record WalkthroughResponse(
    [property: JsonPropertyName("walkthroughs")]
    List<Walkthrough> Walkthroughs);

public record Walkthrough(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("name")]
    string Name);
