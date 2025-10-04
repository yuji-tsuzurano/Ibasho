using Ibasho.Application.DTOs;
using Ibasho.Data;
using Ibasho.Data.Entities;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// 投稿のEF実装
/// </summary>
/// <param name="db">EF Core のアプリケーションDBコンテキスト。</param>
public sealed class PostRepository(ApplicationDbContext db) : IPostRepository
{
    /// <summary>
    /// フォロー中ユーザーの直近の投稿を取得
    /// </summary>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    public async Task<IReadOnlyList<PostListItemDto>> GetHomeAsync(string currentUserId, int limit, CancellationToken ct = default)
    {
        var followeeIds = db.Follows
            .AsNoTracking()
            .Where(f => f.FollowerId == currentUserId)
            .Select(f => f.FolloweeId);

        var q = db.Posts.AsNoTracking()
            .Where(p => p.ParentPostId == null && (p.UserId == currentUserId || followeeIds.Contains(p.UserId)))
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit)
            .Select(p => new PostListItemDto
            {
                PostId = p.Id,
                UserId = p.UserId,
                UserDisplayName = p.User.DisplayName,
                UserAvatarUrl = p.User.AvatarUrl,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                ReplyCount = db.Posts.Count(r => r.ParentPostId == p.Id),
                LikeCount = db.PostLikes.Count(l => l.PostId == p.Id),
                IsLikedByCurrentUser = db.PostLikes.Any(l => l.PostId == p.Id && l.UserId == currentUserId)
            });

        return await q.ToListAsync(ct);
    }

    /// <summary>
    /// スレッド（親投稿とその返信）を取得
    /// </summary>
    /// <param name="rootPostId">スレッドの親投稿ID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>親投稿、返信一覧</returns>
    public async Task<(PostListItemDto? Parent, IReadOnlyList<PostListItemDto> Thread)> GetThreadAsync(long rootPostId, string currentUserId, int limit, CancellationToken ct = default)
    {
        var parent = await db.Posts.AsNoTracking()
            .Where(p => p.Id == rootPostId)
            .Select(p => new PostListItemDto
            {
                PostId = p.Id,
                UserId = p.UserId,
                UserDisplayName = p.User.DisplayName,
                UserAvatarUrl = p.User.AvatarUrl,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                ReplyCount = db.Posts.Count(r => r.ParentPostId == p.Id),
                LikeCount = db.PostLikes.Count(l => l.PostId == p.Id),
                IsLikedByCurrentUser = db.PostLikes.Any(l => l.PostId == p.Id && l.UserId == currentUserId)
            })
            .FirstOrDefaultAsync(ct);

        var thread = await db.Posts.AsNoTracking()
            .Where(p => p.ParentPostId == rootPostId)
            .OrderBy(p => p.CreatedAt)
            .Take(limit)
            .Select(p => new PostListItemDto
            {
                PostId = p.Id,
                UserId = p.UserId,
                UserDisplayName = p.User.DisplayName,
                UserAvatarUrl = p.User.AvatarUrl,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                ReplyCount = db.Posts.Count(r => r.ParentPostId == p.Id),
                LikeCount = db.PostLikes.Count(l => l.PostId == p.Id),
                IsLikedByCurrentUser = db.PostLikes.Any(l => l.PostId == p.Id && l.UserId == currentUserId)
            })
            .ToListAsync(ct);

        return (parent, thread);
    }

    /// <summary>
    /// 指定ユーザーの直近投稿を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    public Task<IReadOnlyList<PostListItemDto>> GetByUserAsync(string targetUserId, string currentUserId, int limit, CancellationToken ct = default)
    {
        var q = db.Posts.AsNoTracking()
            .Where(p => p.ParentPostId == null && p.UserId == targetUserId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit)
            .Select(p => new PostListItemDto
            {
                PostId = p.Id,
                UserId = p.UserId,
                UserDisplayName = p.User.DisplayName,
                UserAvatarUrl = p.User.AvatarUrl,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                ReplyCount = db.Posts.Count(r => r.ParentPostId == p.Id),
                LikeCount = db.PostLikes.Count(l => l.PostId == p.Id),
                IsLikedByCurrentUser = db.PostLikes.Any(l => l.PostId == p.Id && l.UserId == currentUserId)
            });

        return q.ToListAsync(ct).ContinueWith(t => (IReadOnlyList<PostListItemDto>)t.Result, ct);
    }

    /// <summary>
    /// 指定ユーザーがいいねした投稿を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    public Task<IReadOnlyList<PostListItemDto>> GetLikedByUserAsync(string targetUserId, string currentUserId, int limit, CancellationToken ct = default)
    {
        var q = db.PostLikes.AsNoTracking()
            .Where(l => l.UserId == targetUserId)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => l.Post)
            .Where(p => p != null && p.ParentPostId == null)
            .OrderByDescending(p => p!.CreatedAt)
            .Take(limit)
            .Select(p => new PostListItemDto
            {
                PostId = p!.Id,
                UserId = p.UserId,
                UserDisplayName = p.User.DisplayName,
                UserAvatarUrl = p.User.AvatarUrl,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                ReplyCount = db.Posts.Count(r => r.ParentPostId == p.Id),
                LikeCount = db.PostLikes.Count(l => l.PostId == p.Id),
                IsLikedByCurrentUser = db.PostLikes.Any(l => l.PostId == p.Id && l.UserId == currentUserId)
            });

        return q.ToListAsync(ct).ContinueWith(t => (IReadOnlyList<PostListItemDto>)t.Result, ct);
    }

    /// <summary>
    /// 新規投稿を作成
    /// </summary>
    /// <param name="post">投稿エンティティ</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>保存された投稿エンティティ</returns>
    public async Task<Post> CreateAsync(Post post, CancellationToken ct = default)
    {
        db.Posts.Add(post);
        await db.SaveChangesAsync(ct);
        return post;
    }

    /// <summary>
    /// 返信投稿を作成
    /// </summary>
    /// <param name="reply">返信エンティティ</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>保存された返信エンティティ</returns>
    public async Task<Post> CreateReplyAsync(Post reply, CancellationToken ct = default)
    {
        db.Posts.Add(reply);
        await db.SaveChangesAsync(ct);
        return reply;
    }
}
