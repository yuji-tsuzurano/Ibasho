using Ibasho.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Application.UseCases.Users;

/// <summary>
/// 投稿検索ユースケース
/// </summary>
/// <param name="postSearch">投稿検索リポジトリ</param>
public sealed class GetUserIdUseCase(UserManager<ApplicationUser> userManager)
{
    /// <summary>
    /// ユーザーIdの取得
    /// </summary>
    /// <param name="displayId">表示用ユーザーId</param>
    /// <returns>ユーザーId</returns>
    public async Task<string?> ExecuteAsync(string displayId)
    {
        if (string.IsNullOrWhiteSpace(displayId))
        {
            return null;
        }

        ApplicationUser? user = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.DisplayUserId == displayId);

        return user?.Id;
    }
}
