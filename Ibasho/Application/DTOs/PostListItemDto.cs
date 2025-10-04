namespace Ibasho.Application.DTOs;

/// <summary>
/// 投稿情報
/// </summary>
public sealed class PostListItemDto
{
    /// 投稿ID
    public long PostId { get; init; }

    /// 投稿者のユーザーID
    public string UserId { get; init; } = string.Empty;

    /// 投稿者の表示名
    public string UserDisplayName { get; init; } = string.Empty;

    /// 投稿者のアバターURL
    public string? UserAvatarUrl { get; init; }

    /// 投稿本文
    public string Content { get; init; } = string.Empty;

    /// 投稿日時
    public DateTime CreatedAt { get; init; }

    /// 返信件数
    public int ReplyCount { get; init; }

    /// いいね件数
    public int LikeCount { get; init; }

    /// 自分がいいね済みか
    public bool IsLikedByCurrentUser { get; init; }
}
