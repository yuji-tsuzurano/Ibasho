using Ibasho.Domain.Entities;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// フォロー操作のEF実装
/// </summary>
/// <param name="db">EF Core のアプリケーションDBコンテキスト。</param>
public sealed class FollowRepository(ApplicationDbContext db) : IFollowRepository
{
    /// <summary>
    /// 指定ユーザーのフォロー状態を切替
    /// </summary>
    /// <param name="followerId">ログインユーザID</param>
    /// <param name="followeeId">フォローするユーザID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>切替後のフォロー状態。true:フォロー中 false:未フォロー</returns>
    public async Task<bool> ToggleAsync(string followerId, string followeeId, CancellationToken ct = default)
    {
        var rel = await db.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId, ct);
        if (rel is not null)
        {
            db.Follows.Remove(rel);
            await db.SaveChangesAsync(ct);
            return false;
        }

        db.Follows.Add(new Follow { FollowerId = followerId, FolloweeId = followeeId, CreatedAt = DateTime.UtcNow });
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
