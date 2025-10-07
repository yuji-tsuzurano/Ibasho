using Microsoft.AspNetCore.Identity;

namespace Ibasho.Domain.Entities
{
    /// <summary>
    /// アプリケーションユーザーエンティティ
    /// ASP.NET Core IdentityのIdentityUserを拡張し、SNS用のプロパティを追加します
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 表示用ユーザーID（必須）
        /// </summary>
        public string DisplayUserId { get; set; } = string.Empty;

        /// <summary>
        /// 表示名（必須）
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 自己紹介文（任意）
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// プロフィール画像のURL（任意）
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// バナー画像のURL（任意）
        /// </summary>
        public string? BannerUrl { get; set; }

        /// <summary>
        /// 作成日時（UTC）
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新日時（UTC）
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties - 初期化を確実に行う
        /// <summary>
        /// ユーザーの投稿一覧
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// ユーザーのいいね一覧
        /// </summary>
        public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

        /// <summary>
        /// フォローしているユーザー一覧
        /// </summary>
        public virtual ICollection<Follow> Following { get; set; } = new List<Follow>();

        /// <summary>
        /// フォロワー一覧
        /// </summary>
        public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();

        /// <summary>
        /// 通知一覧
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
