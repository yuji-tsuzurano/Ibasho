namespace Ibasho.Application.DTOs;

/// <summary>
/// プロフィール情報
/// </summary>
public sealed class ProfileInfoDto
{
    /// 対象ユーザーID
    public string UserId { get; init; } = string.Empty;

    /// 表示名
    public string DisplayName { get; init; } = string.Empty;

    /// 自己紹介
    public string? Bio { get; init; }

    /// アバターURL
    public string? AvatarUrl { get; init; }

    /// バナーURL
    public string? BannerUrl { get; init; }

    /// フォロー数
    public int FollowingCount { get; init; }

    /// フォロワー数
    public int FollowerCount { get; init; }

    /// 投稿数（親投稿のみ）
    public int PostCount { get; init; }

    /// 現在ユーザーが対象をフォローしているか
    public bool IsFollowedByCurrentUser { get; init; }
}
