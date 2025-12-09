using Ibasho.Application.DTOs;

namespace Ibasho.Components.Partials.Container.Profile;

/// <summary>
/// プロフィール画面の状態
/// </summary>
public sealed class ProfileContainerState
{
    /// <summary>
    /// キャッシュキー
    /// </summary>
    public const string CacheKey = "ProfileContainerState";

    /// <summary>
    /// アクティブタブ
    /// </summary>
    public string ActiveTab { get; set; } = "投稿";

    /// <summary>
    /// 投稿一覧
    /// </summary>
    public IReadOnlyList<PostListItemDto> Posts { get; set; } = [];
}
