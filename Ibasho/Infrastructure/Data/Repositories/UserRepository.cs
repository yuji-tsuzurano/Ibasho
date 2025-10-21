using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// ユーザーのEF実装
/// </summary>
public sealed class UserRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : IUserRepository
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
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);
        ProfileInfoDto? user = await db.Users.AsNoTracking()
            .Where(u => u.Id == targetUserId)
            .Select(u => new ProfileInfoDto
            {
                UserId = u.Id,
                DisplayUserId = u.DisplayUserId,
                DisplayName = u.DisplayName,
                Bio = u.Bio,
                AvatarUrl = u.AvatarUrl,
                BannerUrl = u.BannerUrl
            })
            .FirstOrDefaultAsync(ct);

        if (user == null)
        {
            return null;
        }

        // 統計情報を取得
        user.FollowingCount = await db.Follows.AsNoTracking().CountAsync(f => f.FollowerId == targetUserId, ct);
        user.FollowerCount = await db.Follows.AsNoTracking().CountAsync(f => f.FolloweeId == targetUserId, ct);
        user.PostCount = await db.Posts.AsNoTracking().CountAsync(p => p.UserId == targetUserId && p.ParentPostId == null, ct);
        user.IsFollowedByCurrentUser = await db.Follows.AsNoTracking().AnyAsync(f => f.FollowerId == currentUserId && f.FolloweeId == targetUserId, ct);

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
    public async Task<IReadOnlyList<UserListItemDto>> SearchUsersAsync(string keyword, string currentUserId, int limit, CancellationToken ct = default)
    {
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);

        keyword = keyword.Trim();
        IQueryable<UserListItemDto> q = db.Users.AsNoTracking()
            .Where(u => EF.Functions.ILike(u.DisplayName, $"%{keyword}%") || EF.Functions.ILike(u.UserName!, $"%{keyword}%"))
            .OrderByDescending(u => u.CreatedAt)
            .Take(limit)
            .Select(u => new UserListItemDto
            {
                UserId = u.Id,
                DisplayUserId = u.DisplayUserId,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Bio = u.Bio,
                IsFollowedByCurrentUser = db.Follows.Any(f => f.FollowerId == currentUserId && f.FolloweeId == u.Id)
            });

        return await q.ToListAsync(ct);
    }
}
