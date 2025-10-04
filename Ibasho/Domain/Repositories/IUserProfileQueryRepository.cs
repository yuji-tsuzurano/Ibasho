namespace Ibasho.Domain.Repositories;

/// <summary>
/// ユーザープロフィール関連情報取得インターフェース
/// </summary>
public interface IUserProfileQueryRepository
{
    /// <summary>
    /// 指定ユーザーのプロフィール情報を取得
    /// </summary>
    /// <param name="targetUserId">対象ユーザーID</param>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>フォロー数・フォロワー数・投稿数・フォロー状態</returns>
    Task<(int following, int followers, int posts, bool isFollowedByCurrentUser)> GetProfileInfoAsync(
        string targetUserId,
        string currentUserId,
        CancellationToken ct = default);
}
