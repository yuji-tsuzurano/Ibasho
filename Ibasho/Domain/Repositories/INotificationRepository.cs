using Ibasho.Application.DTOs;

namespace Ibasho.Domain.Repositories;

/// <summary>
/// 通知操作インターフェース
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// 指定ユーザーの直近の通知を取得
    /// </summary>
    /// <param name="userId">取得対象のユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>通知情報一覧</returns>
    Task<IReadOnlyList<NotificationItemDto>> GetRecentAsync(string userId, int limit, CancellationToken ct = default);

    /// <summary>
    /// 指定ユーザーの未読通知をすべて既読に更新
    /// </summary>
    /// <param name="userId">取得対象のユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>タスク</returns>
    Task MarkAllReadAsync(string userId, CancellationToken ct = default);
}
