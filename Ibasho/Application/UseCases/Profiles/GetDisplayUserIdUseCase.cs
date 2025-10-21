using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Profiles;

/// <summary>
/// 表示用ユーザID取得ユースケース。
/// </summary>
/// <param name="users">ユーザー取得用リポジトリ。</param>
public sealed class GetDisplayUserIdUseCase(IUserRepository users)
{
    /// <summary>
    /// 表示用ユーザID取得
    /// </summary>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>プロフィール情報</returns>
    public async Task<string> ExecuteAsync(string currentUserId, CancellationToken ct = default)
    {
        ProfileInfoDto? profileInfo = await users.GetProfileAsync(currentUserId, currentUserId, ct);
        return profileInfo == null ? "" : profileInfo.DisplayUserId;
    }
}
