using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNextPlantoSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NextPlanId",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NextTenantPlanId",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "NextPlanId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "NextTenantPlanId",
                table: "Subscriptions");
        }
    }
}
