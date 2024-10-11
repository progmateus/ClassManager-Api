using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class renameTenantSocialsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantSocials_Tenants_TenantId",
                table: "TenantSocials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantSocials",
                table: "TenantSocials");

            migrationBuilder.RenameTable(
                name: "TenantSocials",
                newName: "TenantsSocials");

            migrationBuilder.RenameIndex(
                name: "IX_TenantSocials_TenantId",
                table: "TenantsSocials",
                newName: "IX_TenantsSocials_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantsSocials",
                table: "TenantsSocials",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantsSocials_Tenants_TenantId",
                table: "TenantsSocials",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantsSocials_Tenants_TenantId",
                table: "TenantsSocials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantsSocials",
                table: "TenantsSocials");

            migrationBuilder.RenameTable(
                name: "TenantsSocials",
                newName: "TenantSocials");

            migrationBuilder.RenameIndex(
                name: "IX_TenantsSocials_TenantId",
                table: "TenantSocials",
                newName: "IX_TenantSocials_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantSocials",
                table: "TenantSocials",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSocials_Tenants_TenantId",
                table: "TenantSocials",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }
    }
}
