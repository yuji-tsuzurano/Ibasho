using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Notifications;

/// <summary>
/// 直近通知取得ユースケース
/// </summary>
/// <param name="notifications">通知取得リポジトリ</param>
public sealed class GetNotificationsUseCase(INotificationRepository notifications)
{
    /// <summary>
    /// 指定ユーザーの直近の通知を取得
    /// </summary>
    /// <param name="userId">取得対象のユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>通知情報一覧</returns>
    public Task<IReadOnlyList<NotificationItemDto>> ExecuteAsync(string userId, int limit = 50, CancellationToken ct = default)
    {
        return notifications.GetRecentAsync(userId, limit, ct);
    }
}
