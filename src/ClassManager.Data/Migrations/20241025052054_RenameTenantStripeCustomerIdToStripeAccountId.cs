using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTenantStripeCustomerIdToStripeAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StripeCustomer_Tenants_TenantId",
                table: "StripeCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_StripeCustomer_Users_UserId",
                table: "StripeCustomer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StripeCustomer",
                table: "StripeCustomer");

            migrationBuilder.RenameTable(
                name: "StripeCustomer",
                newName: "StripeCustomers");

            migrationBuilder.RenameColumn(
                name: "StripeCustomerId",
                table: "Tenants",
                newName: "StripeAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_StripeCustomer_UserId",
                table: "StripeCustomers",
                newName: "IX_StripeCustomers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StripeCustomer_TenantId",
                table: "StripeCustomers",
                newName: "IX_StripeCustomers_TenantId");

            migrationBuilder.AlterColumn<string>(
                name: "StripeCustomerId",
                table: "StripeCustomers",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StripeCustomers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StripeCustomers",
                table: "StripeCustomers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StripeCustomers_Tenants_TenantId",
                table: "StripeCustomers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StripeCustomers_Users_UserId",
                table: "StripeCustomers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StripeCustomers_Tenants_TenantId",
                table: "StripeCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_StripeCustomers_Users_UserId",
                table: "StripeCustomers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StripeCustomers",
                table: "StripeCustomers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StripeCustomers");

            migrationBuilder.RenameTable(
                name: "StripeCustomers",
                newName: "StripeCustomer");

            migrationBuilder.RenameColumn(
                name: "StripeAccountId",
                table: "Tenants",
                newName: "StripeCustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_StripeCustomers_UserId",
                table: "StripeCustomer",
                newName: "IX_StripeCustomer_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StripeCustomers_TenantId",
                table: "StripeCustomer",
                newName: "IX_StripeCustomer_TenantId");

            migrationBuilder.AlterColumn<string>(
                name: "StripeCustomerId",
                table: "StripeCustomer",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StripeCustomer",
                table: "StripeCustomer",
                column: "StripeCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StripeCustomer_Tenants_TenantId",
                table: "StripeCustomer",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StripeCustomer_Users_UserId",
                table: "StripeCustomer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
