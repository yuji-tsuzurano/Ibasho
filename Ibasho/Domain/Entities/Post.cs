using System.ComponentModel.DataAnnotations;

namespace Ibasho.Domain.Entities
{
    /// <summary>
    /// 投稿エンティティ
    /// ユーザーの投稿やリプライを管理します
    /// </summary>
    public class Post
    {
        /// <summary>
        /// 投稿ID（主キー）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 投稿者のユーザーID
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 投稿内容（最大280文字）
        /// </summary>
        [Required]
        [MaxLength(280)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 親投稿ID（リプライの場合に設定）
        /// </summary>
        public long? ParentPostId { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 削除フラグ
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        /// <summary>
        /// 投稿者（ユーザー情報）
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// 親投稿（リプライ元の投稿）
        /// </summary>
        public Post? ParentPost { get; set; }

        /// <summary>
        /// リプライ一覧
        /// </summary>
        public ICollection<Post> Replies { get; set; } = new List<Post>();

        /// <summary>
        /// いいね一覧
        /// </summary>
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}