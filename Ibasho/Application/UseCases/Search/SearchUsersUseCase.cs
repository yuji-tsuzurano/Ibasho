using Ibasho.Application.DTOs;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Search;

/// <summary>
/// ユーザー検索ユースケース
/// </summary>
/// <param name="users">ユーザー検索リポジトリ</param>
public sealed class SearchUsersUseCase(IUserRepository users)
{
    /// <summary>
    /// キーワードに部分一致するユーザーを検索
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>ユーザー一覧</returns>
    public Task<IReadOnlyList<UserListItemDto>> ExecuteAsync(string keyword, string currentUserId, int limit = 50, CancellationToken ct = default)
    {
        return users.SearchUsersAsync(keyword, currentUserId, limit, ct);
    }
}
