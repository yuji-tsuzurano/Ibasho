using Ibasho.Application.DTOs;

namespace Ibasho.Components.Partials.Container.Notifications;

/// <summary>
/// 通知画面の状態
/// ブラウザのセッションストレージに保存される
/// </summary>
public sealed class NotificationsContainerState
{
    /// <summary>
    /// キャッシュキー
    /// </summary>
    public const string CacheKey = "NotificationsContainerState";

    /// <summary>
    /// 通知一覧
    /// </summary>
    public IReadOnlyList<NotificationItemDto> Notifications { get; set; } = [];
}
