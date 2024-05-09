using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditBusinnesHour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BusinessHour",
                table: "Classes",
                type: "VARCHAR(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BusinessHour",
                table: "Classes",
                type: "VARCHAR",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
