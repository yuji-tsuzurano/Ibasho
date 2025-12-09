using Ibasho.Application.DTOs;

namespace Ibasho.Components.Partials.Container.Home;

/// <summary>
/// ホーム画面の状態
/// </summary>
public sealed class HomeContainerState
{
    /// <summary>
    /// キャッシュキー
    /// </summary>
    public const string CacheKey = "HomeContainerState";

    /// <summary>
    /// 投稿一覧
    /// </summary>
    public List<PostListItemDto> Posts { get; set; } = new();
}
