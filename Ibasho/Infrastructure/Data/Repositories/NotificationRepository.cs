using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Repositories;

/// <summary>
/// 通知操作のEF実装
/// </summary>
/// <param name="dbFactory">DbContextのファクトリ</param>
public sealed class NotificationRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : INotificationRepository
{
    /// <summary>
    /// 指定ユーザーの直近の通知を取得
    /// </summary>
    /// <param name="userId">取得対象のユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>通知情報一覧</returns>
    public async Task<IReadOnlyList<NotificationItemDto>> GetRecentAsync(string userId, int limit, CancellationToken ct = default)
    {
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);
        IQueryable<NotificationItemDto> q = db.Notifications.AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .Select(n => new NotificationItemDto
            {
                NotificationId = n.Id,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                ActorUserId = n.ActorUserId,
                ActorDisplayUserId = n.ActorUser.DisplayUserId,
                ActorDisplayName = n.ActorUser.DisplayName,
                ActorAvatarUrl = n.ActorUser.AvatarUrl,
                PostId = n.PostId,
                Message = n.Message
            });

        return await q.ToListAsync(ct);
    }

    /// <summary>
    /// 指定ユーザーの未読通知をすべて既読に更新
    /// </summary>
    /// <param name="userId">取得対象のユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>タスク</returns>
    public async Task MarkAllReadAsync(string userId, CancellationToken ct = default)
    {
        await using ApplicationDbContext db = await dbFactory.CreateDbContextAsync(ct);
        _ = await db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct);
    }
}
