namespace Scighost.PixivApi.Models.Fanbox;

/// <summary>
/// Wrapper for the plans response from plan.listCreator and plan.listSupporting endpoints
/// </summary>
/// <param name="Plans">Array of supporting plans</param>
public record CreatorPlansResponse(
    [property: JsonPropertyName("plans")]
    SupportingPlan[] Plans
);
