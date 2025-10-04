using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Search;

/// <summary>
/// 投稿検索ユースケース
/// </summary>
/// <param name="postSearch">投稿検索リポジトリ</param>
public sealed class SearchPostsUseCase(IPostSearchRepository postSearch)
{
    /// <summary>
    /// キーワードに部分一致する投稿を検索
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>投稿一覧</returns>
    public Task<IReadOnlyList<PostListItemDto>> ExecuteAsync(string keyword, string currentUserId, int limit = 50, CancellationToken ct = default)
    {
        return postSearch.SearchAsync(keyword, currentUserId, limit, ct);
    }
}
