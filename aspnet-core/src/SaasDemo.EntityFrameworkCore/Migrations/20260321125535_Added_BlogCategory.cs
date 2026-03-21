using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaasDemo.Migrations
{
    /// <inheritdoc />
    public partial class Added_BlogCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppBlogCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_AppBlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppBlogPostCategories",
                columns: table => new
                {
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBlogPostCategories", x => new { x.BlogPostId, x.BlogCategoryId });
                    table.ForeignKey(
                        name: "FK_AppBlogPostCategories_AppBlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "AppBlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppBlogPostCategories_AppBlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "AppBlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBlogPostCategories_BlogCategoryId",
                table: "AppBlogPostCategories",
                column: "BlogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBlogPostCategories_BlogPostId_BlogCategoryId",
                table: "AppBlogPostCategories",
                columns: new[] { "BlogPostId", "BlogCategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBlogPostCategories");

            migrationBuilder.DropTable(
                name: "AppBlogCategories");
        }
    }
}
