using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Profiles;

/// <summary>
/// ユーザーがいいねした投稿取得ユースケース
/// </summary>
/// <param name="posts">投稿取得リポジトリ</param>
public sealed class GetUserLikesUseCase(IPostRepository posts)
{
    /// <summary>
    /// 指定ユーザーがいいねした投稿を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    public Task<IReadOnlyList<PostListItemDto>> ExecuteAsync(string targetUserId, string currentUserId, int limit = 50, CancellationToken ct = default)
    {
        return posts.GetLikedByUserAsync(targetUserId, currentUserId, limit, ct);
    }
}
