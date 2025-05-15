namespace Scighost.PixivApi.V2;

/// <summary>
/// 
/// </summary>
/// <param name="AccessToken"></param>
/// <param name="ExpiresIn"></param>
/// <param name="TokenType"></param>
/// <param name="Scope"></param>
/// <param name="RefreshToken"></param>
/// <param name="User"></param>
public record OauthLoginResponse(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("expires_in")]
    uint ExpiresIn,
    [property: JsonPropertyName("token_type")]
    string TokenType,
    [property: JsonPropertyName("scope")]
    string Scope,
    [property: JsonPropertyName("refresh_token")]
    string RefreshToken,
    [property: JsonPropertyName("user")]
    User User
);