using Ibasho.Domain.Entities;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// 投稿のいいね操作のEF実装
/// </summary>
/// <param name="dbFactory">DbContextのファクトリ</param>
public sealed class PostLikeRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : IPostLikeRepository
{
    /// <summary>
    /// いいね状態の切替
    /// </summary>
    /// <param name="postId">対象の投稿ID</param>
    /// <param name="userId">対象のユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>切替後のいいね状態（true:いいね false:未いいね）</returns>
    public async Task<bool> ToggleAsync(long postId, string userId, CancellationToken ct = default)
    {
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);
        PostLike? like = await db.PostLikes.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId, ct);
        if (like is not null)
        {
            _ = db.PostLikes.Remove(like);
            _ = await db.SaveChangesAsync(ct);
            return false;
        }

        _ = db.PostLikes.Add(new PostLike { PostId = postId, UserId = userId, CreatedAt = DateTime.UtcNow });
        try
        {
            _ = await db.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateException)
        {
            return true;
        }
    }

    /// <summary>
    /// 投稿のいいね数を取得
    /// </summary>
    /// <param name="postId">対象の投稿ID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿のいいね数</returns>
    public async Task<int> CountAsync(long postId, CancellationToken ct = default)
    {
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);
        return await db.PostLikes.CountAsync(x => x.PostId == postId, ct);
    }
}
