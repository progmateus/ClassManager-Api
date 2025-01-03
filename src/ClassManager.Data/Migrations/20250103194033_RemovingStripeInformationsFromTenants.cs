using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingStripeInformationsFromTenants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPeriodEnd",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CurrentPeriodStart",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ExpiresDate",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SubscriptionStatus",
                table: "Tenants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentPeriodEnd",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentPeriodStart",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresDate",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "Tenants",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SubscriptionStatus",
                table: "Tenants",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)1);
        }
    }
}
