using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLatestInvoiceToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LatestInvoiceId",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_LatestInvoiceId",
                table: "Subscriptions",
                column: "LatestInvoiceId",
                unique: true,
                filter: "[LatestInvoiceId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Invoices_LatestInvoiceId",
                table: "Subscriptions",
                column: "LatestInvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Invoices_LatestInvoiceId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_LatestInvoiceId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "LatestInvoiceId",
                table: "Subscriptions");
        }
    }
}
