using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeToStripeCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "StripeCustomers",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "StripeCustomers");
        }
    }
}
