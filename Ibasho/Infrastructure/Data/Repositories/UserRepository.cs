using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// ユーザーのEF実装
/// </summary>
public sealed class UserRepository(ApplicationDbContext db) : IUserRepository
{
    /// <summary>
    /// 指定ユーザーのプロフィール情報を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>プロフィール情報</returns>
    public async Task<ProfileInfoDto?> GetProfileAsync(string targetUserId, string currentUserId, CancellationToken ct = default)
    {
        var user = await db.Users.AsNoTracking()
            .Where(u => u.Id == targetUserId)
            .Select(u => new ProfileInfoDto
            {
                UserId = u.Id,
                DisplayName = u.DisplayName,
                Bio = u.Bio,
                AvatarUrl = u.AvatarUrl,
                BannerUrl = u.BannerUrl,
                FollowingCount = db.Follows.Count(f => f.FollowerId == u.Id),
                FollowerCount = db.Follows.Count(f => f.FolloweeId == u.Id),
                PostCount = db.Posts.Count(p => p.UserId == u.Id && p.ParentPostId == null),
                IsFollowedByCurrentUser = db.Follows.Any(f => f.FollowerId == currentUserId && f.FolloweeId == u.Id)
            })
            .FirstOrDefaultAsync(ct);

        return user;
    }

    /// <summary>
    /// キーワードに部分一致するユーザーを検索
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>ユーザー一覧</returns>
    public Task<IReadOnlyList<UserListItemDto>> SearchUsersAsync(string keyword, string currentUserId, int limit, CancellationToken ct = default)
    {
        keyword = keyword.Trim();
        var q = db.Users.AsNoTracking()
            .Where(u => EF.Functions.ILike(u.DisplayName, $"%{keyword}%") || EF.Functions.ILike(u.UserName!, $"%{keyword}%"))
            .OrderByDescending(u => u.CreatedAt)
            .Take(limit)
            .Select(u => new UserListItemDto
            {
                UserId = u.Id,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Bio = u.Bio,
                IsFollowedByCurrentUser = db.Follows.Any(f => f.FollowerId == currentUserId && f.FolloweeId == u.Id)
            });

        return q.ToListAsync(ct).ContinueWith(t => (IReadOnlyList<UserListItemDto>)t.Result, ct);
    }
}
