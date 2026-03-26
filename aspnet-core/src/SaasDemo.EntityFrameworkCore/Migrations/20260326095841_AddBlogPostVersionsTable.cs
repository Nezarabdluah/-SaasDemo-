using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaasDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogPostVersionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppBlogPostVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    OgImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBlogPostVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBlogPostVersions_AppBlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "AppBlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBlogPostVersions_BlogPostId_VersionNumber",
                table: "AppBlogPostVersions",
                columns: new[] { "BlogPostId", "VersionNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBlogPostVersions");
        }
    }
}
