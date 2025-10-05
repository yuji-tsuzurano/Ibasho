using Ibasho.Application.DTOs;
using Ibasho.Domain.Entities;

namespace Ibasho.Domain.Repositories;

/// <summary>
/// 投稿操作インターフェース
/// </summary>
public interface IPostRepository
{
    /// <summary>
    /// 直近の投稿を取得
    /// </summary>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    Task<IReadOnlyList<PostListItemDto>> GetHomeAsync(string currentUserId, int limit, CancellationToken ct = default);

    /// <summary>
    /// スレッド（親投稿とその返信）を取得
    /// </summary>
    /// <param name="rootPostId">スレッドの親投稿ID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>親投稿、返信一覧</returns>
    Task<(PostListItemDto? Parent, IReadOnlyList<PostListItemDto> Thread)> GetThreadAsync(long rootPostId, string currentUserId, int limit, CancellationToken ct = default);

    /// <summary>
    /// 指定ユーザーの直近投稿を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    Task<IReadOnlyList<PostListItemDto>> GetByUserAsync(string targetUserId, string currentUserId, int limit, CancellationToken ct = default);

    /// <summary>
    /// 指定ユーザーがいいねした投稿を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    Task<IReadOnlyList<PostListItemDto>> GetLikedByUserAsync(string targetUserId, string currentUserId, int limit, CancellationToken ct = default);

    /// <summary>
    /// 新規投稿を作成
    /// </summary>
    /// <param name="post">投稿エンティティ</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>保存された投稿エンティティ</returns>
    Task<Post> CreateAsync(Post post, CancellationToken ct = default);

    /// <summary>
    /// 返信投稿を作成
    /// </summary>
    /// <param name="reply">返信エンティティ</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>保存された返信エンティティ</returns>
    Task<Post> CreateReplyAsync(Post reply, CancellationToken ct = default);
}
