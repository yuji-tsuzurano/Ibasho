using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Follows;

/// <summary>
/// フォロー切替ユースケース
/// </summary>
/// <param name="follows">フォロー操作リポジトリ</param>
public sealed class ToggleFollowUseCase(IFollowRepository follows)
{
    /// <summary>
    /// 指定ユーザのフォロー状態を切替
    /// </summary>
    /// <param name="followerId">ログインユーザID</param>
    /// <param name="followeeId">フォローするユーザID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>切替後のフォロー状態。true:フォロー中 false:未フォロー</returns>
    public Task<bool> ExecuteAsync(string followerId, string followeeId, CancellationToken ct = default)
    {
        return follows.ToggleAsync(followerId, followeeId, ct);
    }
}
