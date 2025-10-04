namespace Ibasho.Domain.Repositories;

/// <summary>
/// 投稿のいいね操作インターフェース
/// </summary>
public interface IPostLikeRepository
{
    /// <summary>
    /// いいね状態の切替
    /// </summary>
    /// <param name="postId">対象の投稿ID</param>
    /// <param name="userId">対象のユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>切替後のいいね状態（true:いいね false:未いいね）</returns>
    Task<bool> ToggleAsync(long postId, string userId, CancellationToken ct = default);
}
