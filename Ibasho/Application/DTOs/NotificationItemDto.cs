using Ibasho.Domain.Enums;

namespace Ibasho.Application.DTOs;

/// <summary>
/// 通知情報
/// </summary>
public sealed class NotificationItemDto
{
    /// 通知ID
    public long NotificationId { get; init; }

    /// 通知種別
    public NotificationType Type { get; init; }

    /// 既読フラグ
    public bool IsRead { get; init; }

    /// 作成日時
    public DateTime CreatedAt { get; init; }

    /// アクターのユーザーID
    public string ActorUserId { get; init; } = string.Empty;

    /// アクターの表示用ユーザーID
    public string ActorDisplayUserId { get; init; } = string.Empty;

    /// アクターの表示名
    public string ActorDisplayName { get; init; } = string.Empty;

    /// アクターのアバターURL
    public string? ActorAvatarUrl { get; init; }

    /// 関連する投稿ID
    public long? PostId { get; init; }

    /// メッセージ本文
    public string? Message { get; init; }
}
