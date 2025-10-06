using Ibasho.Application.UseCases.Follows;
using Ibasho.Application.UseCases.Likes;
using Ibasho.Application.UseCases.Notifications;
using Ibasho.Application.UseCases.Posts;
using Ibasho.Application.UseCases.Profiles;
using Ibasho.Application.UseCases.Replies;
using Ibasho.Application.UseCases.Search;
using Ibasho.Components;
using Ibasho.Components.Account;
using Ibasho.Components.Pages.Login;
using Ibasho.Domain.Entities;
using Ibasho.Domain.Repositories;
using Ibasho.Infrastructure.Data;
using Ibasho.Infrastructure.Data.Repositories;
using Ibasho.Infrastructure.Data.Seeding;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ローカル専用設定 (非公開) を追加読み込み: appsettings.Local.json (存在する場合のみ)
builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = Login.Path;
});

// ドメイン契約に対するEF実装のDI登録
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostSearchRepository, PostSearchRepository>();
builder.Services.AddScoped<IPostLikeRepository, PostLikeRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileQueryRepository, UserProfileQueryRepository>();

// ユースケースのDI登録
builder.Services.AddScoped<GetHomeTimelineUseCase>();
builder.Services.AddScoped<CreatePostUseCase>();
builder.Services.AddScoped<SearchPostsUseCase>();
builder.Services.AddScoped<ToggleLikeUseCase>();
builder.Services.AddScoped<GetThreadUseCase>();
builder.Services.AddScoped<CreateReplyUseCase>();
builder.Services.AddScoped<GetUserProfileUseCase>();
builder.Services.AddScoped<GetUserPostsUseCase>();
builder.Services.AddScoped<GetUserLikesUseCase>();
builder.Services.AddScoped<ToggleFollowUseCase>();
builder.Services.AddScoped<GetNotificationsUseCase>();
builder.Services.AddScoped<MarkAllNotificationsReadUseCase>();
builder.Services.AddScoped<SearchUsersUseCase>();

WebApplication app = builder.Build();

// ========================================
// テストデータ作成処理（appsettings.jsonで制御）
// ========================================
bool enableSeedData = builder.Configuration.GetValue<bool>("SeedData:EnableSeedData");
int userCount = builder.Configuration.GetValue<int>("SeedData:UserCount", 10);
int postCount = builder.Configuration.GetValue<int>("SeedData:PostCount", 50);

if (enableSeedData)
{
    Console.WriteLine("=== テストデータ作成モード ===");
    Console.WriteLine($"設定: ユーザー数={userCount}, 投稿数={postCount}");

    try
    {
        using IServiceScope scope = app.Services.CreateScope();
        await SeedData.InitializeAsync(scope.ServiceProvider, userCount, postCount);
        Console.WriteLine("✅ テストデータの作成が完了しました！");
        Console.WriteLine("📝 appsettings.jsonのSeedData:EnableSeedDataをfalseに戻すことを忘れずに。");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ テストデータ作成中にエラーが発生しました: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"詳細: {ex.InnerException.Message}");
        }
    }

    Console.WriteLine("テストデータ作成処理を終了します。");
    return; // アプリケーションを終了
}

// ========================================
// 通常のアプリケーション起動処理
// ========================================

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseMigrationsEndPoint();
}
else
{
    _ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
