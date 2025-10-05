using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// ユーザープロフィール関連情報取得のEF実装
/// </summary>
/// <param name="db">EF Core のアプリケーションDBコンテキスト</param>
public sealed class UserProfileQueryRepository(ApplicationDbContext db) : IUserProfileQueryRepository
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
        var followingTask = db.Follows.AsNoTracking().CountAsync(f => f.FollowerId == targetUserId, ct);
        var followersTask = db.Follows.AsNoTracking().CountAsync(f => f.FolloweeId == targetUserId, ct);
        var postsTask = db.Posts.AsNoTracking().CountAsync(p => p.UserId == targetUserId && p.ParentPostId == null, ct);
        var followedTask = db.Follows.AsNoTracking().AnyAsync(f => f.FollowerId == currentUserId && f.FolloweeId == targetUserId, ct);

        await Task.WhenAll(followingTask, followersTask, postsTask, followedTask);
        return (followingTask.Result, followersTask.Result, postsTask.Result, followedTask.Result);
    }
}
