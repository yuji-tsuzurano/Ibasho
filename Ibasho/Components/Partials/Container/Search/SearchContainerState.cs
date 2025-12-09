using Ibasho.Application.DTOs;

namespace Ibasho.Components.Partials.Container.Search;

/// <summary>
/// 検索画面の状態
/// ブラウザのセッションストレージに保存される
/// </summary>
public sealed class SearchContainerState
{
    /// <summary>
    /// キャッシュキー
    /// </summary>
    public const string CacheKey = "SearchContainerState";

    /// <summary>
    /// 検索キーワード
    /// </summary>
    public string Keyword { get; set; } = "";

    /// <summary>
    /// アクティブタブ
    /// </summary>
    public string ActiveTab { get; set; } = "ユーザー";

    /// <summary>
    /// ユーザー検索結果
    /// </summary>
    public IReadOnlyList<UserListItemDto> Users { get; set; } = [];

    /// <summary>
    /// 投稿検索結果
    /// </summary>
    public IReadOnlyList<PostListItemDto> Posts { get; set; } = [];
}
