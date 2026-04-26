namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// 
/// </summary>
/// <param name="Count">Total item count</param>
/// <param name="Items">50 per page</param>
/// <param name="NextPage">Null if no more pages</param>
public record CreatorSearchResult(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("creators")] FollowedCreator[] Items,
    [property: JsonPropertyName("nextPage")] int? NextPage
);