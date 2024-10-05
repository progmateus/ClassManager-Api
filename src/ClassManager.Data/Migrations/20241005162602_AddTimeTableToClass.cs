using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeTableToClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TimeTableId",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TimeTableId",
                table: "Classes",
                column: "TimeTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_TimesTables_TimeTableId",
                table: "Classes",
                column: "TimeTableId",
                principalTable: "TimesTables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_TimesTables_TimeTableId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TimeTableId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "TimeTableId",
                table: "Classes");
        }
    }
}
