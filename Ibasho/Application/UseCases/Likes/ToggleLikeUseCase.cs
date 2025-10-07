using Ibasho.Domain.Repositories;
using Ibasho.Application.DTOs.Likes;

namespace Ibasho.Application.UseCases.Likes;

/// <summary>
/// 投稿のいいね切替ユースケース
/// </summary>
/// <param name="likes">いいね操作リポジトリ</param>
public sealed class ToggleLikeUseCase(IPostLikeRepository likes)
{
    /// <summary>
    /// いいね状態の切替といいね数取得
    /// </summary>
    /// <param name="postId">対象の投稿ID</param>
    /// <param name="userId">対象のユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>切替後のいいね状態といいね数</returns>
    public async Task<ToggleLikeResultDto> ExecuteAsync(long postId, string userId, CancellationToken ct = default)
    {
        // いいね状態の切替
        bool isLiked = await likes.ToggleAsync(postId, userId, ct);
        // 投稿のいいね数取得
        int likeCount = await likes.CountAsync(postId, ct);
        // DTOで返却
        return new ToggleLikeResultDto
        {
            IsLiked = isLiked,
            LikeCount = likeCount
        };
    }
}
