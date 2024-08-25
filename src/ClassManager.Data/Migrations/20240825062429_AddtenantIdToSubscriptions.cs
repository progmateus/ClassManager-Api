using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddtenantIdToSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Users",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)3,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)1);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Tenants",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)3,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)1);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Subscriptions",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)2);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_TenantId",
                table: "Subscriptions",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Tenants_TenantId",
                table: "Subscriptions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Tenants_TenantId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_TenantId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Users",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)3);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Tenants",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)3);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Subscriptions",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)2,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)1);
        }
    }
}
