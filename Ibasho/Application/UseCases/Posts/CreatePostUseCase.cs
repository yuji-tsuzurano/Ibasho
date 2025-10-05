using Ibasho.Domain.Entities;
using Ibasho.Domain.Repositories;

namespace Ibasho.Application.UseCases.Posts;

/// <summary>
/// 新規投稿作成ユースケース
/// </summary>
/// <param name="posts">投稿作成リポジトリ</param>
public sealed class CreatePostUseCase(IPostRepository posts)
{
    /// <summary>
    /// 新規投稿を作成
    /// </summary>
    /// <param name="currentUserId">ログインユーザーID</param>
    /// <param name="content">投稿本文</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>保存された投稿エンティティ</returns>
    public Task<Post> ExecuteAsync(string currentUserId, string content, CancellationToken ct = default)
    {
        return posts.CreateAsync(new Post { UserId = currentUserId, Content = content, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, ct);
    }
}
