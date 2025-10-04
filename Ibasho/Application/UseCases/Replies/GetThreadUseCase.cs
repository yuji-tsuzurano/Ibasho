using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Replies;

/// <summary>
/// 親投稿とスレッド取得ユースケース
/// </summary>
/// <param name="posts">投稿取得リポジトリ</param>
public sealed class GetThreadUseCase(IPostRepository posts)
{
    /// <summary>
    /// スレッド（親投稿とその返信）を取得
    /// </summary>
    /// <param name="rootPostId">スレッドの親投稿ID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>親投稿、返信一覧</returns>
    public Task<(PostListItemDto? Parent, IReadOnlyList<PostListItemDto> Thread)> ExecuteAsync(long rootPostId, string currentUserId, int limit = 50, CancellationToken ct = default)
    {
        return posts.GetThreadAsync(rootPostId, currentUserId, limit, ct);
    }
}
