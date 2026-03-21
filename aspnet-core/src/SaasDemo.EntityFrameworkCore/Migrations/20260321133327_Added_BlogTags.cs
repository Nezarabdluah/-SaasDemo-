using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaasDemo.Migrations
{
    /// <inheritdoc />
    public partial class Added_BlogTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppBlogTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBlogTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppBlogPostTags",
                columns: table => new
                {
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBlogPostTags", x => new { x.BlogPostId, x.BlogTagId });
                    table.ForeignKey(
                        name: "FK_AppBlogPostTags_AppBlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "AppBlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppBlogPostTags_AppBlogTags_BlogTagId",
                        column: x => x.BlogTagId,
                        principalTable: "AppBlogTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBlogPostTags_BlogPostId_BlogTagId",
                table: "AppBlogPostTags",
                columns: new[] { "BlogPostId", "BlogTagId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppBlogPostTags_BlogTagId",
                table: "AppBlogPostTags",
                column: "BlogTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBlogPostTags");

            migrationBuilder.DropTable(
                name: "AppBlogTags");
        }
    }
}
