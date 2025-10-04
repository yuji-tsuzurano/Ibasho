using Ibasho.Data;
using Ibasho.Data.Entities;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// 投稿のいいね操作のEF実装
/// </summary>
/// <param name="db">EF Core のアプリケーションDBコンテキスト。</param>
public sealed class PostLikeRepository(ApplicationDbContext db) : IPostLikeRepository
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
        var like = await db.PostLikes.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId, ct);
        if (like is not null)
        {
            db.PostLikes.Remove(like);
            await db.SaveChangesAsync(ct);
            return false;
        }

        db.PostLikes.Add(new PostLike { PostId = postId, UserId = userId, CreatedAt = DateTime.UtcNow });
        try
        {
            await db.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateException)
        {
            return true;
        }
    }
}
