using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// 投稿検索のEF実装
/// </summary>
/// <param name="dbFactory">DbContextのファクトリ</param>
public sealed class PostSearchRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : IPostSearchRepository
{
    /// <summary>
    /// キーワードに部分一致する投稿を検索
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    public async Task<IReadOnlyList<PostListItemDto>> SearchAsync(string keyword, string currentUserId, int limit, CancellationToken ct = default)
    {
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);
        IQueryable<PostListItemDto> q = db.Posts.AsNoTracking()
            .Where(p => p.ParentPostId == null && p.Content.Contains(keyword))
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit)
            .Select(p => new PostListItemDto
            {
                PostId = p.Id,
                UserId = p.UserId,
                DisplayUserId = p.User.DisplayUserId,
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
}
