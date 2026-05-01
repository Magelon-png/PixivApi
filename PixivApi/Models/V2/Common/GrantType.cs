using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Scighost.PixivApi.Models.V2.Common;

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