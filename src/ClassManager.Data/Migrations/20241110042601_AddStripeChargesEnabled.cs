using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeChargesEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StripeChargesEnabled",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeChargesEnabled",
                table: "Tenants");

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
    }
}
