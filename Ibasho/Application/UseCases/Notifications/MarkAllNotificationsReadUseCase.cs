using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Notifications;

/// <summary>
/// 通知の全件既読化ユースケース
/// </summary>
/// <param name="notifications">通知更新リポジトリ</param>
public sealed class MarkAllNotificationsReadUseCase(INotificationRepository notifications)
{
    /// <summary>
    /// 指定ユーザーの未読通知をすべて既読に更新
    /// </summary>
    /// <param name="userId">取得対象のユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>タスク</returns>
    public Task ExecuteAsync(string userId, CancellationToken ct = default)
    {
        return notifications.MarkAllReadAsync(userId, ct);
    }
}
