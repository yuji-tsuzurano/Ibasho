using System.ComponentModel.DataAnnotations;

namespace Ibasho.Domain.Entities
{
    /// <summary>
    /// フォロー関係エンティティ
    /// ユーザー間のフォロー関係を管理します
    /// </summary>
    public class Follow
    {
        /// <summary>
        /// フォロー関係ID（主キー）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// フォローする人のユーザーID
        /// </summary>
        [Required]
        public string FollowerId { get; set; } = string.Empty;

        /// <summary>
        /// フォローされる人のユーザーID
        /// </summary>
        [Required]
        public string FolloweeId { get; set; } = string.Empty;

        /// <summary>
        /// フォロー開始日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        /// <summary>
        /// フォローする人（ユーザー情報）
        /// </summary>
        public ApplicationUser Follower { get; set; } = null!;

        /// <summary>
        /// フォローされる人（ユーザー情報）
        /// </summary>
        public ApplicationUser Followee { get; set; } = null!;
    }
}