using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Likes;

/// <summary>
/// 投稿のいいね切替ユースケース
/// </summary>
/// <param name="likes">いいね操作リポジトリ</param>
public sealed class ToggleLikeUseCase(IPostLikeRepository likes)
{
    /// <summary>
    /// いいね状態の切替
    /// </summary>
    /// <param name="postId">対象の投稿ID</param>
    /// <param name="userId">対象のユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>切替後のいいね状態（true:いいね false:未いいね）</returns>
    public Task<bool> ExecuteAsync(long postId, string userId, CancellationToken ct = default)
    {
        return likes.ToggleAsync(postId, userId, ct);
    }
}
