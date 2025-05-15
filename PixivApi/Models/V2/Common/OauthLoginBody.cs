namespace Scighost.PixivApi.V2;

/// <summary>
/// 
/// </summary>
/// <param name="ClientId"></param>
/// <param name="ClientSecret"></param>
/// <param name="Code">Required if grant type is authorization code</param>
/// <param name="CodeVerifier">Required if grant type is authorization code</param>
/// <param name="GrantType"></param>
/// <param name="RedirectUri"></param>
/// <param name="RefreshToken">Required if grant type is refresh token</param>
public record OauthLoginBody(string ClientId, string ClientSecret, string? Code, 
    string? CodeVerifier, GrantType GrantType, string RedirectUri, string? RefreshToken)
{

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> ToDictionary()
    {
        return GrantType switch
        {
            GrantType.AuthorizationCode => new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "code", Code ?? throw new ArgumentNullException(nameof(Code))},
                { "code_verifier", CodeVerifier ?? throw new ArgumentNullException(nameof(CodeVerifier)) },
                { "grant_type", GrantType.ToStringFast(true) },
                { "include_policy", "true" },
                { "redirect_uri", RedirectUri },
            },
            GrantType.RefreshToken => new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "grant_type", GrantType.ToStringFast(true) },
                { "include_policy", "true" },
                { "refresh_token", RefreshToken ?? throw new ArgumentNullException(nameof(RefreshToken)) }
            },
            _ => throw new ArgumentException("Grant type provided is not supported.", nameof(GrantType))
        };
    }
};