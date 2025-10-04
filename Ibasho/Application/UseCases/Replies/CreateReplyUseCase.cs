using Ibasho.Data.Entities;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Replies;

/// <summary>
/// 返信作成ユースケース
/// </summary>
/// <param name="posts">返信作成リポジトリ</param>
public sealed class CreateReplyUseCase(IPostRepository posts)
{
    /// <summary>
    /// 親投稿に対して返信を作成
    /// </summary>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="parentPostId">親投稿ID</param>
    /// <param name="content">返信本文</param>
    /// <param name="ct">キャンセル用トークン</param>
    /// <returns>作成された返信エンティティ</returns>
    public Task<Post> ExecuteAsync(string currentUserId, long parentPostId, string content, CancellationToken ct = default)
    {
        return posts.CreateReplyAsync(new Post { UserId = currentUserId, ParentPostId = parentPostId, Content = content, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, ct);
    }
}
