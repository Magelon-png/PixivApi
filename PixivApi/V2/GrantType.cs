using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.V2;

/// <summary>
/// 
/// </summary>
[EnumExtensions]
public enum GrantType
{
    /// <summary>
    /// authorization_code
    /// </summary>
    [Display(Name = "authorization_code")]
    AuthorizationCode,
    /// <summary>
    /// refresh_token
    /// </summary>
    [Display(Name = "refresh_token")]
    RefreshToken,
}