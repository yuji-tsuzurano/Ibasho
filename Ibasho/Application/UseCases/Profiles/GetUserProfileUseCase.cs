using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Profiles;

/// <summary>
/// プロフィール情報取得ユースケース。
/// </summary>
/// <param name="users">ユーザー取得用リポジトリ。</param>
public sealed class GetUserProfileUseCase(IUserRepository users)
{
    /// <summary>
    /// 指定ユーザーのプロフィール情報を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>プロフィール情報</returns>
    public Task<ProfileInfoDto?> ExecuteAsync(string targetUserId, string currentUserId, CancellationToken ct = default)
    {
        return users.GetProfileAsync(targetUserId, currentUserId, ct);
    }
}
