using Ibasho.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ibasho.Infrastructure.Data
{
    /// <summary>
    /// アプリケーションのデータベースコンテキスト
    /// ASP.NET Core IdentityとSNS機能のエンティティを管理します
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        /// <summary>
        /// 投稿エンティティのDbSet
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// 投稿いいねエンティティのDbSet
        /// </summary>
        public DbSet<PostLike> PostLikes { get; set; }

        /// <summary>
        /// フォロー関係エンティティのDbSet
        /// </summary>
        public DbSet<Follow> Follows { get; set; }

        /// <summary>
        /// 通知エンティティのDbSet
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// モデル作成時の設定
        /// エンティティ間のリレーションとPostgreSQL用の設定を行います
        /// </summary>
        /// <param name="builder">モデルビルダー</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureApplicationUser(builder);
            ConfigurePost(builder);
            ConfigurePostLike(builder);
            ConfigureFollow(builder);
            ConfigureNotification(builder);

            ApplyPostgresSnakeCaseConventions(builder);
        }

        /// <summary>
        /// PostgreSQL 向けにテーブル / カラム / 制約 / インデックス名を snake_case & 規則化
        /// </summary>
        private static void ApplyPostgresSnakeCaseConventions(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // テーブル名
                var tableName = entity.GetTableName();
                if (!string.IsNullOrEmpty(tableName))
                {
                    var newTableName = ToSnakeCase(tableName);
                    entity.SetTableName(newTableName);

                    // 主キー
                    var pk = entity.FindPrimaryKey();
                    if (pk != null)
                    {
                        pk.SetName($"pk_{newTableName}");
                    }

                    // 外部キー
                    foreach (var fk in entity.GetForeignKeys())
                    {
                        // 依存カラム
                        var depCols = string.Join("_", fk.Properties.Select(p => ToSnakeCase(p.Name)));
                        var principalTable = ToSnakeCase(fk.PrincipalEntityType.GetTableName() ?? fk.PrincipalEntityType.ClrType.Name);
                        fk.SetConstraintName($"fk_{newTableName}_{principalTable}_{depCols}");
                    }

                    // インデックス
                    foreach (var index in entity.GetIndexes())
                    {
                        var cols = string.Join("_", index.Properties.Select(p => ToSnakeCase(p.Name)));
                        var prefix = index.IsUnique ? "ux" : "ix";
                        index.SetDatabaseName($"{prefix}_{newTableName}_{cols}");
                    }
                }

                // カラム
                var storeObject = StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema());
                foreach (var prop in entity.GetProperties())
                {
                    var original = prop.GetColumnName(storeObject);
                    if (!string.IsNullOrEmpty(original))
                    {
                        prop.SetColumnName(ToSnakeCase(original));
                    }
                }
            }
        }

        /// <summary>
        /// PascalCase / camelCase / MixedCase → snake_case (全小文字)
        /// </summary>
        private static string ToSnakeCase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            var sb = new System.Text.StringBuilder(name.Length + 8);
            var prevLowerOrDigit = false;

            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i];

                if (char.IsUpper(c))
                {
                    if (i > 0 && prevLowerOrDigit)
                        sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                    prevLowerOrDigit = false;
                }
                else if (c == '-' || c == ' ')
                {
                    sb.Append('_');
                    prevLowerOrDigit = false;
                }
                else
                {
                    if (c == '_')
                    {
                        sb.Append('_');
                        prevLowerOrDigit = false;
                    }
                    else
                    {
                        sb.Append(c);
                        prevLowerOrDigit = char.IsLower(c) || char.IsDigit(c);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// ApplicationUserエンティティの設定を行います
        /// </summary>
        /// <param name="builder">モデルビルダー</param>
        private static void ConfigureApplicationUser(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity =>
            {
                // インデックス設定
                entity.HasIndex(u => u.DisplayName)
                    .HasDatabaseName("IX_Users_DisplayName");

                entity.HasIndex(u => u.CreatedAt)
                    .HasDatabaseName("IX_Users_CreatedAt");
            });
        }

        /// <summary>
        /// Postエンティティの設定を行います
        /// </summary>
        /// <param name="builder">モデルビルダー</param>
        private static void ConfigurePost(ModelBuilder builder)
        {
            builder.Entity<Post>(entity =>
            {
                // リレーション設定
                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.ParentPost)
                    .WithMany(p => p.Replies)
                    .HasForeignKey(p => p.ParentPostId)
                    .OnDelete(DeleteBehavior.Restrict);

                // インデックス設定
                entity.HasIndex(p => p.UserId)
                    .HasDatabaseName("IX_Posts_UserId");

                entity.HasIndex(p => p.CreatedAt)
                    .HasDatabaseName("IX_Posts_CreatedAt");

                entity.HasIndex(p => new { p.UserId, p.CreatedAt })
                    .HasDatabaseName("IX_Posts_UserId_CreatedAt");

                entity.HasIndex(p => p.ParentPostId)
                    .HasDatabaseName("IX_Posts_ParentPostId");
            });
        }

        /// <summary>
        /// PostLikeエンティティの設定を行います
        /// </summary>
        /// <param name="builder">モデルビルダー</param>
        private static void ConfigurePostLike(ModelBuilder builder)
        {
            builder.Entity<PostLike>(entity =>
            {
                // リレーション設定
                entity.HasOne(pl => pl.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(pl => pl.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pl => pl.User)
                    .WithMany(u => u.PostLikes)
                    .HasForeignKey(pl => pl.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // 制約設定
                entity.HasIndex(pl => new { pl.PostId, pl.UserId })
                    .IsUnique();

                // インデックス設定
                entity.HasIndex(pl => pl.PostId)
                    .HasDatabaseName("IX_PostLikes_PostId");

                entity.HasIndex(pl => pl.UserId)
                    .HasDatabaseName("IX_PostLikes_UserId");

                entity.HasIndex(pl => new { pl.UserId, pl.CreatedAt })
                    .HasDatabaseName("IX_PostLikes_UserId_CreatedAt");
            });
        }

        /// <summary>
        /// Followエンティティの設定を行います
        /// </summary>
        /// <param name="builder">モデルビルダー</param>
        private static void ConfigureFollow(ModelBuilder builder)
        {
            builder.Entity<Follow>(entity =>
            {
                // リレーション設定
                entity.HasOne(f => f.Follower)
                    .WithMany(u => u.Following)
                    .HasForeignKey(f => f.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Followee)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(f => f.FolloweeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // 制約設定
                entity.HasIndex(f => new { f.FollowerId, f.FolloweeId })
                    .IsUnique();

                // インデックス設定
                entity.HasIndex(f => f.FollowerId)
                    .HasDatabaseName("IX_Follows_FollowerId");

                entity.HasIndex(f => f.FolloweeId)
                    .HasDatabaseName("IX_Follows_FolloweeId");
            });
        }

        /// <summary>
        /// Notificationエンティティの設定を行います
        /// </summary>
        /// <param name="builder">モデルビルダー</param>
        private static void ConfigureNotification(ModelBuilder builder)
        {
            builder.Entity<Notification>(entity =>
            {
                // リレーション設定
                entity.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.ActorUser)
                    .WithMany()
                    .HasForeignKey(n => n.ActorUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Post)
                    .WithMany()
                    .HasForeignKey(n => n.PostId)
                    .OnDelete(DeleteBehavior.SetNull);

                // インデックス設定
                entity.HasIndex(n => n.UserId)
                    .HasDatabaseName("IX_Notifications_UserId");

                entity.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt })
                    .HasDatabaseName("IX_Notifications_UserId_IsRead_CreatedAt");

                entity.HasIndex(n => new { n.UserId, n.CreatedAt })
                    .HasDatabaseName("IX_Notifications_UserId_CreatedAt");
            });
        }
    }
}
