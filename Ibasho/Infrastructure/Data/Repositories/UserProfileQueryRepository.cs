using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// ユーザープロフィール関連情報取得のEF実装
/// </summary>
/// <param name="dbFactory">DbContextのファクトリ</param>
public sealed class UserProfileQueryRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : IUserProfileQueryRepository
{
    /// <summary>
    /// 指定ユーザーのプロフィール情報を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>フォロー数・フォロワー数・投稿数・フォロー状態</returns>
    public async Task<(int following, int followers, int posts, bool isFollowedByCurrentUser)> GetProfileInfoAsync(
        string targetUserId,
        string currentUserId,
        CancellationToken ct = default)
    {
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);

        int following = await db.Follows.AsNoTracking().CountAsync(f => f.FollowerId == targetUserId, ct);
        int followers = await db.Follows.AsNoTracking().CountAsync(f => f.FolloweeId == targetUserId, ct);
        int posts = await db.Posts.AsNoTracking().CountAsync(p => p.UserId == targetUserId && p.ParentPostId == null, ct);
        bool isFollowed = await db.Follows.AsNoTracking().AnyAsync(f => f.FollowerId == currentUserId && f.FolloweeId == targetUserId, ct);

        return (following, followers, posts, isFollowed);
    }
}
