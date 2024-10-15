using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeAndTargetTypeToInvoiceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "DECIMAL(18,0)", nullable: false),
                    Status = table.Column<byte>(type: "TINYINT", nullable: false, defaultValue: (byte)1),
                    TargetType = table.Column<byte>(type: "TINYINT", nullable: false, defaultValue: (byte)1),
                    Type = table.Column<byte>(type: "TINYINT", nullable: false, defaultValue: (byte)1),
                    ExpiresAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersInvoices_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersInvoices_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersInvoices_TenantPlans_TenantPlanId",
                        column: x => x.TenantPlanId,
                        principalTable: "TenantPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersInvoices_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersInvoices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersInvoices_PlanId",
                table: "UsersInvoices",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInvoices_SubscriptionId",
                table: "UsersInvoices",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInvoices_TenantId",
                table: "UsersInvoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInvoices_TenantPlanId",
                table: "UsersInvoices",
                column: "TenantPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInvoices_UserId",
                table: "UsersInvoices",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersInvoices");
        }
    }
}
