namespace Ibasho.Application.DTOs;

/// <summary>
/// ユーザー情報
/// </summary>
public sealed class UserListItemDto
{
    /// ユーザーID
    public string UserId { get; init; } = string.Empty;

    /// 表示名
    public string DisplayName { get; init; } = string.Empty;

    /// アバターURL
    public string? AvatarUrl { get; init; }

    /// 自己紹介
    public string? Bio { get; init; }

    /// 現在ユーザーが対象をフォローしているか
    public bool IsFollowedByCurrentUser { get; init; }
}
