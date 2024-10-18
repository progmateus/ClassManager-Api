using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSubscriptionStatusDefaultvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Subscriptions",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)4,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Subscriptions",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)4);
        }
    }
}
