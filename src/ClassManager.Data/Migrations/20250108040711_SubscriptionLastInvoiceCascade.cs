using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SubscriptionLastInvoiceCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Invoices_LatestInvoiceId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Plans_NextPlanId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_TenantPlans_NextTenantPlanId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_NextPlanId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_NextTenantPlanId",
                table: "Subscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_NextPlanId",
                table: "Subscriptions",
                column: "NextPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_NextTenantPlanId",
                table: "Subscriptions",
                column: "NextTenantPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Invoices_LatestInvoiceId",
                table: "Subscriptions",
                column: "LatestInvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Plans_NextPlanId",
                table: "Subscriptions",
                column: "NextPlanId",
                principalTable: "Plans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_TenantPlans_NextTenantPlanId",
                table: "Subscriptions",
                column: "NextTenantPlanId",
                principalTable: "TenantPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Invoices_LatestInvoiceId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Plans_NextPlanId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_TenantPlans_NextTenantPlanId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_NextPlanId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_NextTenantPlanId",
                table: "Subscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_NextPlanId",
                table: "Subscriptions",
                column: "NextPlanId",
                unique: true,
                filter: "[NextPlanId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_NextTenantPlanId",
                table: "Subscriptions",
                column: "NextTenantPlanId",
                unique: true,
                filter: "[NextTenantPlanId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Invoices_LatestInvoiceId",
                table: "Subscriptions",
                column: "LatestInvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Plans_NextPlanId",
                table: "Subscriptions",
                column: "NextPlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_TenantPlans_NextTenantPlanId",
                table: "Subscriptions",
                column: "NextTenantPlanId",
                principalTable: "TenantPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
