using System.ComponentModel.DataAnnotations;

namespace Ibasho.Domain.Entities
{
    /// <summary>
    /// 投稿いいねエンティティ
    /// ユーザーが投稿に対して行ういいねを管理します
    /// </summary>
    public class PostLike
    {
        /// <summary>
        /// いいねID（主キー）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// いいね対象の投稿ID
        /// </summary>
        [Required]
        public long PostId { get; set; }

        /// <summary>
        /// いいねしたユーザーID
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// いいね作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        /// <summary>
        /// いいね対象の投稿
        /// </summary>
        public Post Post { get; set; } = null!;

        /// <summary>
        /// いいねしたユーザー
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
    }
}