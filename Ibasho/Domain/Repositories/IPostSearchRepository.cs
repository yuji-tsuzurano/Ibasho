using Ibasho.Application.DTOs;

namespace Ibasho.Domain.Repositories;

/// <summary>
/// 投稿検索操作インターフェース
/// </summary>
public interface IPostSearchRepository
{
    /// <summary>
    /// キーワードに部分一致する投稿を検索
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    Task<IReadOnlyList<PostListItemDto>> SearchAsync(string keyword, string currentUserId, int limit, CancellationToken ct = default);
}
