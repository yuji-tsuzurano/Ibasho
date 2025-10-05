using Ibasho.Domain.Entities;
using Ibasho.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ibasho.Infrastructure.Data.Seeding
{
    /// <summary>
    /// データベースの初期データを作成するクラス
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// 初期データを作成します
        /// </summary>
        /// <param name="serviceProvider">サービスプロバイダー</param>
        /// <param name="userCount">作成するユーザー数</param>
        /// <param name="postCount">作成する投稿数</param>
        public static async Task InitializeAsync(IServiceProvider serviceProvider, int userCount = 10, int postCount = 50)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 既存データを削除
            await ClearAllDataAsync(context);

            Console.WriteLine($"初期データ作成を開始します... (ユーザー数: {userCount}, 投稿数: {postCount})");

            await CreateUsersAsync(userManager, context, userCount);
            await CreatePostsAsync(context, postCount);
            await CreateFollowsAsync(context, userCount);
            await CreatePostLikesAsync(context, postCount);
            await CreateNotificationsAsync(context, userCount);

            Console.WriteLine("初期データ作成が完了しました！");
        }

        /// <summary>
        /// シンプルなテストユーザーを作成します
        /// </summary>
        /// <param name="userManager">ユーザーマネージャー</param>
        /// <param name="context">データベースコンテキスト</param>
        /// <param name="userCount">作成するユーザー数</param>
        private static async Task CreateUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, int userCount)
        {
            var users = new List<ApplicationUser>();

            // 全てのユーザーを統一的に作成
            for (int i = 0; i < userCount; i++)
            {
                var userNumber = i + 1;
                var user = new ApplicationUser
                {
                    UserName = $"testuser{userNumber}@example.com",
                    Email = $"testuser{userNumber}@example.com",
                    DisplayName = $"テストユーザー{userNumber}",
                    Bio = $"テストユーザー{userNumber}です。よろしくお願いします。",
                    AvatarUrl = $"/images/avatars/default{(i % 5) + 1}.png",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-(i + 1)) // 1日間隔で作成
                };

                var result = await userManager.CreateAsync(user, "TestPass123!");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"ユーザー作成に失敗しました: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                
                users.Add(user);
                Console.WriteLine($"ユーザー作成: {user.DisplayName} ({user.UserName})");
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"合計 {userCount} 人のユーザーを作成しました。");
        }

        /// <summary>
        /// テスト投稿を作成します
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        /// <param name="postCount">作成する投稿数</param>
        private static async Task CreatePostsAsync(ApplicationDbContext context, int postCount)
        {
            var users = await context.Users.OrderBy(u => u.CreatedAt).ToListAsync();
            var posts = new List<Post>();

            // シンプルな投稿テキスト
            var sampleTexts = new[]
            {
                "今日はいい天気ですね！",
                "プログラミング勉強中です。",
                "新しいカフェを見つけました。",
                "映画を見に行きました。",
                "散歩してきました。",
                "美味しいラーメンを食べました🍜",
                "桜が綺麗に咲いています🌸",
                "今日は早起きできました！",
                "新しい本を読み始めました�",
                "散歩していたら猫に会いました🐱",
                "コーヒーが美味しい季節ですね",
                "今日の夕日が綺麗でした🌅",
                "新しいレシピに挑戦中👨‍🍳",
                "音楽を聞きながらリラックス🎵",
                "友達と楽しい時間を過ごしました👥",
                "C#の新機能を試しています",
                "Entity Frameworkの勉強中です",
                "PostgreSQLのパフォーマンスチューニング学習中🚀",
                "新しいUIデザインが完成しました🎨",
                "テストコードを書く習慣をつけました",
                "GitHubでオープンソース活動始めました",
                "技術書を読んで知識を深めています",
                "新しいフレームワークを試しています"
            };

            // 投稿を作成
            for (int i = 0; i < postCount; i++)
            {
                var userIndex = i % users.Count; // ユーザーを順番に使う
                var textIndex = i % sampleTexts.Length; // テキストを順番に使う
                
                var post = new Post
                {
                    UserId = users[userIndex].Id,
                    Content = $"{sampleTexts[textIndex]} #{i + 1:D3}",
                    CreatedAt = DateTime.UtcNow.AddDays(-i), // 1日ずつ過去に
                    UpdatedAt = DateTime.UtcNow.AddDays(-i)
                };
                
                posts.Add(post);
            }
            
            context.Posts.AddRange(posts);
            await context.SaveChangesAsync();
            Console.WriteLine($"合計 {postCount} 件の投稿を作成しました。");
        }

        /// <summary>
        /// ハードコードでフォロー関係を作成します（3件固定）
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        /// <param name="userCount">ユーザー数（参考値）</param>
        private static async Task CreateFollowsAsync(ApplicationDbContext context, int userCount)
        {
            var users = await context.Users.OrderBy(u => u.CreatedAt).Take(3).ToListAsync();
            
            if (users.Count < 3)
            {
                Console.WriteLine("フォロー関係を作成するには、少なくとも3人のユーザーが必要です。");
                return;
            }

            var follows = new List<Follow>
            {
                // テストユーザー1 → テストユーザー2
                new Follow
                {
                    FollowerId = users[0].Id,
                    FolloweeId = users[1].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                // テストユーザー2 → テストユーザー3
                new Follow
                {
                    FollowerId = users[1].Id,
                    FolloweeId = users[2].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                // テストユーザー3 → テストユーザー1
                new Follow
                {
                    FollowerId = users[2].Id,
                    FolloweeId = users[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            context.Follows.AddRange(follows);
            await context.SaveChangesAsync();
            Console.WriteLine($"合計 {follows.Count} 件のフォロー関係を作成しました。");
        }

        /// <summary>
        /// ハードコードでいいねを作成します（3件固定）
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        /// <param name="postCount">投稿数（参考値）</param>
        private static async Task CreatePostLikesAsync(ApplicationDbContext context, int postCount)
        {
            var users = await context.Users.OrderBy(u => u.CreatedAt).Take(3).ToListAsync();
            var posts = await context.Posts.Where(p => p.ParentPostId == null).OrderBy(p => p.CreatedAt).Take(3).ToListAsync();
            
            if (!posts.Any() || users.Count < 3)
            {
                Console.WriteLine("いいねを作成するためのデータが不足しています。");
                return;
            }

            var postLikes = new List<PostLike>
            {
                // テストユーザー2がテストユーザー1の投稿にいいね
                new PostLike
                {
                    PostId = posts[0].Id,
                    UserId = users[1].Id,
                    CreatedAt = posts[0].CreatedAt.AddMinutes(30)
                },
                // テストユーザー3がテストユーザー2の投稿にいいね
                new PostLike
                {
                    PostId = posts[1].Id,
                    UserId = users[2].Id,
                    CreatedAt = posts[1].CreatedAt.AddMinutes(45)
                },
                // テストユーザー1がテストユーザー3の投稿にいいね
                new PostLike
                {
                    PostId = posts[2].Id,
                    UserId = users[0].Id,
                    CreatedAt = posts[2].CreatedAt.AddMinutes(60)
                }
            };

            context.PostLikes.AddRange(postLikes);
            await context.SaveChangesAsync();
            Console.WriteLine($"合計 {postLikes.Count} 件のいいねを作成しました。");
        }

        /// <summary>
        /// ハードコードで通知を作成します（3件固定）
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        /// <param name="userCount">ユーザー数（参考値）</param>
        private static async Task CreateNotificationsAsync(ApplicationDbContext context, int userCount)
        {
            var users = await context.Users.OrderBy(u => u.CreatedAt).Take(3).ToListAsync();
            var posts = await context.Posts.Where(p => p.ParentPostId == null).OrderBy(p => p.CreatedAt).Take(3).ToListAsync();

            if (users.Count < 3)
            {
                Console.WriteLine("通知を作成するためのデータが不足しています。");
                return;
            }

            var notifications = new List<Notification>
            {
                new Notification
                {
                    UserId = users[0].Id,
                    ActorUserId = users[1].Id,
                    Type = NotificationType.Follow,
                    Message = "テストユーザー2があなたをフォローしました。",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddHours(-2)
                },
                new Notification
                {
                    UserId = users[0].Id,
                    ActorUserId = users[2].Id,
                    Type = NotificationType.Like,
                    Message = "テストユーザー3があなたの投稿にいいねしました。",
                    PostId = posts.FirstOrDefault()?.Id,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddHours(-1)
                },
                new Notification
                {
                    UserId = users[0].Id,
                    ActorUserId = users[1].Id,
                    Type = NotificationType.Reply,
                    Message = "居場所SNSへようこそ！",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30)
                },
                new Notification
                {
                    UserId = users[0].Id,
                    ActorUserId = users[2].Id,
                    Type = NotificationType.Mention,
                    Message = "居場所SNSへようこそ！",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30)
                }
            };

            context.Notifications.AddRange(notifications);
            await context.SaveChangesAsync();
            Console.WriteLine($"合計 {notifications.Count} 件の通知を作成しました。");
        }

        /// <summary>
        /// 既存の全データを削除します
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        private static async Task ClearAllDataAsync(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("既存データの削除を開始します...");

                // 外部キー制約を考慮して、子テーブルから順番に削除

                // 通知を削除
                var notifications = await context.Notifications.ToListAsync();
                if (notifications.Any())
                {
                    context.Notifications.RemoveRange(notifications);
                    Console.WriteLine($"{notifications.Count} 件の通知を削除しました。");
                }

                // いいねを削除
                var postLikes = await context.PostLikes.ToListAsync();
                if (postLikes.Any())
                {
                    context.PostLikes.RemoveRange(postLikes);
                    Console.WriteLine($"{postLikes.Count} 件のいいねを削除しました。");
                }

                // フォロー関係を削除
                var follows = await context.Follows.ToListAsync();
                if (follows.Any())
                {
                    context.Follows.RemoveRange(follows);
                    Console.WriteLine($"{follows.Count} 件のフォロー関係を削除しました。");
                }

                // 投稿を削除
                var posts = await context.Posts.ToListAsync();
                if (posts.Any())
                {
                    context.Posts.RemoveRange(posts);
                    Console.WriteLine($"{posts.Count} 件の投稿を削除しました。");
                }

                // ユーザーを削除（AspNetUsersテーブル）
                var users = await context.Users.ToListAsync();
                if (users.Any())
                {
                    context.Users.RemoveRange(users);
                    Console.WriteLine($"{users.Count} 件のユーザーを削除しました。");
                }

                // 変更を保存
                await context.SaveChangesAsync();
                Console.WriteLine("既存データの削除が完了しました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"データ削除中にエラーが発生しました: {ex.Message}");
                throw;
            }
        }
    }
}