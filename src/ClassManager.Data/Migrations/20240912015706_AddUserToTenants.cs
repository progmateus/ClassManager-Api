﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToTenants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UserId",
                table: "Tenants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Users_UserId",
                table: "Tenants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Users_UserId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_UserId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tenants");
        }
    }
}
