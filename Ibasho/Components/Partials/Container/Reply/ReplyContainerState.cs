using Ibasho.Application.DTOs;

namespace Ibasho.Components.Partials.Container.Reply;

/// <summary>
/// 返信画面の状態
/// </summary>
public sealed class ReplyContainerState
{
    /// <summary>
    /// キャッシュキー
    /// </summary>
    public const string CacheKey = "ReplyContainerState";

    /// <summary>
    /// 親投稿
    /// </summary>
    public PostListItemDto? ParentPost { get; set; }

    /// <summary>
    /// 返信一覧
    /// </summary>
    public List<PostListItemDto> Posts { get; set; } = new();
}
