using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ibasho.Domain.Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "asp_net_roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            _ = migrationBuilder.CreateTable(
                name: "asp_net_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    banner_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_asp_net_users", x => x.id);
                });

            _ = migrationBuilder.CreateTable(
                name: "asp_net_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "asp_net_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "asp_net_user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    _ = table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "asp_net_user_roles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    _ = table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "asp_net_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    _ = table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "follows",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    follower_id = table.Column<string>(type: "text", nullable: false),
                    followee_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_follows", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_follows_asp_net_users_followee_id",
                        column: x => x.followee_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "fk_follows_asp_net_users_follower_id",
                        column: x => x.follower_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "character varying(280)", maxLength: 280, nullable: false),
                    parent_post_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_posts", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_posts_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "fk_posts_posts_parent_post_id",
                        column: x => x.parent_post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    post_id = table.Column<long>(type: "bigint", nullable: true),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_notifications", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_notifications_asp_net_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "fk_notifications_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "fk_notifications_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            _ = migrationBuilder.CreateTable(
                name: "post_likes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("pk_post_likes", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_post_likes_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "fk_post_likes_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "asp_net_role_claims",
                column: "role_id");

            _ = migrationBuilder.CreateIndex(
                name: "ux_asp_net_roles_normalized_name",
                table: "asp_net_roles",
                column: "normalized_name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "asp_net_user_claims",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "asp_net_user_logins",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "asp_net_user_roles",
                column: "role_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_created_at",
                table: "asp_net_users",
                column: "created_at");

            _ = migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_display_name",
                table: "asp_net_users",
                column: "display_name");

            _ = migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_normalized_email",
                table: "asp_net_users",
                column: "normalized_email");

            _ = migrationBuilder.CreateIndex(
                name: "ux_asp_net_users_normalized_user_name",
                table: "asp_net_users",
                column: "normalized_user_name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "ix_follows_followee_id",
                table: "follows",
                column: "followee_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_follows_follower_id",
                table: "follows",
                column: "follower_id");

            _ = migrationBuilder.CreateIndex(
                name: "ux_follows_follower_id_followee_id",
                table: "follows",
                columns: new[] { "follower_id", "followee_id" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "ix_notifications_actor_user_id",
                table: "notifications",
                column: "actor_user_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_notifications_post_id",
                table: "notifications",
                column: "post_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                table: "notifications",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id_created_at",
                table: "notifications",
                columns: new[] { "user_id", "created_at" });

            _ = migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id_is_read_created_at",
                table: "notifications",
                columns: new[] { "user_id", "is_read", "created_at" });

            _ = migrationBuilder.CreateIndex(
                name: "ix_post_likes_post_id",
                table: "post_likes",
                column: "post_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_post_likes_user_id",
                table: "post_likes",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_post_likes_user_id_created_at",
                table: "post_likes",
                columns: new[] { "user_id", "created_at" });

            _ = migrationBuilder.CreateIndex(
                name: "ux_post_likes_post_id_user_id",
                table: "post_likes",
                columns: new[] { "post_id", "user_id" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "ix_posts_created_at",
                table: "posts",
                column: "created_at");

            _ = migrationBuilder.CreateIndex(
                name: "ix_posts_parent_post_id",
                table: "posts",
                column: "parent_post_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_posts_user_id",
                table: "posts",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "ix_posts_user_id_created_at",
                table: "posts",
                columns: new[] { "user_id", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "asp_net_role_claims");

            _ = migrationBuilder.DropTable(
                name: "asp_net_user_claims");

            _ = migrationBuilder.DropTable(
                name: "asp_net_user_logins");

            _ = migrationBuilder.DropTable(
                name: "asp_net_user_roles");

            _ = migrationBuilder.DropTable(
                name: "asp_net_user_tokens");

            _ = migrationBuilder.DropTable(
                name: "follows");

            _ = migrationBuilder.DropTable(
                name: "notifications");

            _ = migrationBuilder.DropTable(
                name: "post_likes");

            _ = migrationBuilder.DropTable(
                name: "asp_net_roles");

            _ = migrationBuilder.DropTable(
                name: "posts");

            _ = migrationBuilder.DropTable(
                name: "asp_net_users");
        }
    }
}
