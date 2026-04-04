using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaasDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSiteSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PrimaryColor = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SecondaryColor = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LogoMediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SmtpHost = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SmtpPort = table.Column<int>(type: "int", nullable: true),
                    SmtpUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SmtpPassword = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SenderEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SenderName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
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
                    table.PrimaryKey("PK_AppSiteSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSiteSettings");
        }
    }
}
