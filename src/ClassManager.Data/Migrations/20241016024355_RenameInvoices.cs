using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersInvoices_Plans_PlanId",
                table: "UsersInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInvoices_Subscriptions_SubscriptionId",
                table: "UsersInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInvoices_TenantPlans_TenantPlanId",
                table: "UsersInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInvoices_Tenants_TenantId",
                table: "UsersInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInvoices_Users_UserId",
                table: "UsersInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersInvoices",
                table: "UsersInvoices");

            migrationBuilder.RenameTable(
                name: "UsersInvoices",
                newName: "Invoices");

            migrationBuilder.RenameIndex(
                name: "IX_UsersInvoices_UserId",
                table: "Invoices",
                newName: "IX_Invoices_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersInvoices_TenantPlanId",
                table: "Invoices",
                newName: "IX_Invoices_TenantPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersInvoices_TenantId",
                table: "Invoices",
                newName: "IX_Invoices_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersInvoices_SubscriptionId",
                table: "Invoices",
                newName: "IX_Invoices_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersInvoices_PlanId",
                table: "Invoices",
                newName: "IX_Invoices_PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Plans_PlanId",
                table: "Invoices",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Subscriptions_SubscriptionId",
                table: "Invoices",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_TenantPlans_TenantPlanId",
                table: "Invoices",
                column: "TenantPlanId",
                principalTable: "TenantPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Tenants_TenantId",
                table: "Invoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_UserId",
                table: "Invoices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Plans_PlanId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Subscriptions_SubscriptionId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_TenantPlans_TenantPlanId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Tenants_TenantId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Users_UserId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "UsersInvoices");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_UserId",
                table: "UsersInvoices",
                newName: "IX_UsersInvoices_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_TenantPlanId",
                table: "UsersInvoices",
                newName: "IX_UsersInvoices_TenantPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_TenantId",
                table: "UsersInvoices",
                newName: "IX_UsersInvoices_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_SubscriptionId",
                table: "UsersInvoices",
                newName: "IX_UsersInvoices_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_PlanId",
                table: "UsersInvoices",
                newName: "IX_UsersInvoices_PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersInvoices",
                table: "UsersInvoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInvoices_Plans_PlanId",
                table: "UsersInvoices",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInvoices_Subscriptions_SubscriptionId",
                table: "UsersInvoices",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInvoices_TenantPlans_TenantPlanId",
                table: "UsersInvoices",
                column: "TenantPlanId",
                principalTable: "TenantPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInvoices_Tenants_TenantId",
                table: "UsersInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInvoices_Users_UserId",
                table: "UsersInvoices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
