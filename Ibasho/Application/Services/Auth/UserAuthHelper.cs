using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Ibasho.Application.Services.Auth
{
    /// <summary>
    /// ユーザ認証情報ヘルパー
    /// </summary>
    public static class UserAuthHelper
    {
        /// <summary>
        /// ユーザーIDを取得
        /// </summary>
        public static async Task<string?> GetUserIdAsync(AuthenticationStateProvider provider)
        {
            AuthenticationState authState = await provider.GetAuthenticationStateAsync();
            ClaimsPrincipal? user = authState.User;
            return user?.FindFirst("sub")?.Value ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
