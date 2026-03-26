using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaasDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddSeoFieldsAndSlugRedirect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "AppBlogPosts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "AppBlogPosts",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "AppBlogPosts",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgImageUrl",
                table: "AppBlogPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReadingTimeMinutes",
                table: "AppBlogPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ViewCount",
                table: "AppBlogPosts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "AppSlugRedirects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldSlug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSlugRedirects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSlugRedirects_AppBlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "AppBlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBlogPosts_Slug",
                table: "AppBlogPosts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSlugRedirects_BlogPostId",
                table: "AppSlugRedirects",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSlugRedirects_OldSlug",
                table: "AppSlugRedirects",
                column: "OldSlug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSlugRedirects");

            migrationBuilder.DropIndex(
                name: "IX_AppBlogPosts_Slug",
                table: "AppBlogPosts");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "AppBlogPosts");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "AppBlogPosts");

            migrationBuilder.DropColumn(
                name: "OgImageUrl",
                table: "AppBlogPosts");

            migrationBuilder.DropColumn(
                name: "ReadingTimeMinutes",
                table: "AppBlogPosts");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "AppBlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "AppBlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
