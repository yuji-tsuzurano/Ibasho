using System.ComponentModel.DataAnnotations;
using Ibasho.Data.Enums;

namespace Ibasho.Data.Entities
{
    /// <summary>
    /// 通知エンティティ
    /// ユーザーへの各種通知を管理します
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// 通知ID（主キー）
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 通知を受け取るユーザーID
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        /// <summary>
        /// 通知の原因となる行動をしたユーザーID
        /// </summary>
        [Required]
        public string ActorUserId { get; set; } = string.Empty;
        
        /// <summary>
        /// 通知の種類
        /// </summary>
        [Required]
        public NotificationType Type { get; set; }
        
        /// <summary>
        /// 関連する投稿ID（いいねやリプライの場合）
        /// </summary>
        public long? PostId { get; set; }
        
        /// <summary>
        /// 通知メッセージ（最大500文字）
        /// </summary>
        [MaxLength(500)]
        public string? Message { get; set; }
        
        /// <summary>
        /// 既読フラグ
        /// </summary>
        public bool IsRead { get; set; } = false;
        
        /// <summary>
        /// 通知作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        /// <summary>
        /// 通知を受け取るユーザー
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// 通知の原因となる行動をしたユーザー
        /// </summary>
        public ApplicationUser ActorUser { get; set; } = null!;
        
        /// <summary>
        /// 関連する投稿
        /// </summary>
        public Post? Post { get; set; }
    }
}