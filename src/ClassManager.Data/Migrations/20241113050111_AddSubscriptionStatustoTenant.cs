using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionStatustoTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "SubscriptionStatus",
                table: "Tenants",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionStatus",
                table: "Tenants");
        }
    }
}
