using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTeacherId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachersClasses_Users_TeacherId",
                table: "TeachersClasses");

            migrationBuilder.DropIndex(
                name: "IX_TeachersClasses_TeacherId",
                table: "TeachersClasses");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "TeachersClasses");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersClasses_UserId",
                table: "TeachersClasses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersClasses_Users_UserId",
                table: "TeachersClasses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachersClasses_Users_UserId",
                table: "TeachersClasses");

            migrationBuilder.DropIndex(
                name: "IX_TeachersClasses_UserId",
                table: "TeachersClasses");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "TeachersClasses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TeachersClasses_TeacherId",
                table: "TeachersClasses",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersClasses_Users_TeacherId",
                table: "TeachersClasses",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
