using Ibasho.Application.DTOs;

namespace Ibasho.Domain.Repositories;

/// <summary>
/// ユーザー操作インターフェース
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// 指定ユーザーのプロフィール情報を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>プロフィール情報</returns>
    Task<ProfileInfoDto?> GetProfileAsync(string targetUserId, string currentUserId, CancellationToken ct = default);

    /// <summary>
    /// キーワードに部分一致するユーザーを検索
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="limit">取得最大件数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>ユーザー一覧</returns>
    Task<IReadOnlyList<UserListItemDto>> SearchUsersAsync(string keyword, string currentUserId, int limit, CancellationToken ct = default);
}
