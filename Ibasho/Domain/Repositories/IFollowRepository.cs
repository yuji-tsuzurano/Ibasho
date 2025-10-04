namespace Ibasho.Domain.Repositories;

/// <summary>
/// フォロー操作インターフェース
/// </summary>
public interface IFollowRepository
{
    /// <summary>
    /// 指定ユーザのフォロー状態を切替
    /// </summary>
    /// <param name="followerId">ログインユーザID</param>
    /// <param name="followeeId">フォローするユーザID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>切替後のフォロー状態。true:フォロー中 false:未フォロー</returns>
    Task<bool> ToggleAsync(string followerId, string followeeId, CancellationToken ct = default);
}
