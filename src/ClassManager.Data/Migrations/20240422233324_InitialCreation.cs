using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(80)", maxLength: 80, nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(80)", maxLength: 80, nullable: false),
                    Document = table.Column<string>(type: "VARCHAR(80)", maxLength: 80, nullable: false),
                    DocumentType = table.Column<byte>(type: "TINYINT", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordResetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    EmailVerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailVerificationExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailVerificationVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Avatar = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    Status = table.Column<byte>(type: "TINYINT", nullable: false, defaultValue: (byte)1),
                    Type = table.Column<byte>(type: "TINYINT", nullable: false, defaultValue: (byte)1),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
