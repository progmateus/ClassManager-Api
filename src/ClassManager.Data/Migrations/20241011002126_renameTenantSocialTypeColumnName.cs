using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class renameTenantSocialTypeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TINYINT",
                table: "TenantsSocials",
                newName: "Type");

            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "TenantsSocials",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TenantsSocials",
                newName: "TINYINT");

            migrationBuilder.AlterColumn<int>(
                name: "TINYINT",
                table: "TenantsSocials",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldDefaultValue: (byte)1);
        }
    }
}
