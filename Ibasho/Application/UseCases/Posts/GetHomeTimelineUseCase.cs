using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Posts;

/// <summary>
/// ホームの直近投稿取得ユースケース
/// </summary>
/// <param name="posts">投稿取得リポジトリ</param>
public sealed class GetHomeTimelineUseCase(IPostRepository posts)
{
    /// <summary>
    /// フォロー中ユーザーの直近の投稿を取得
    /// </summary>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    public Task<IReadOnlyList<PostListItemDto>> ExecuteAsync(string currentUserId, int limit = 50, CancellationToken ct = default)
    {
        return posts.GetHomeAsync(currentUserId, limit, ct);
    }
}
