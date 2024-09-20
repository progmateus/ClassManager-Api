using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSchedulesDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schedules",
                table: "TimesTables");

            migrationBuilder.CreateTable(
                name: "SchedulesDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeTableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekDay = table.Column<int>(type: "int", nullable: false),
                    HourStart = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    HourEnd = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulesDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulesDays_TimesTables_TimeTableId",
                        column: x => x.TimeTableId,
                        principalTable: "TimesTables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchedulesDays_TimeTableId",
                table: "SchedulesDays",
                column: "TimeTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchedulesDays");

            migrationBuilder.AddColumn<string>(
                name: "Schedules",
                table: "TimesTables",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
