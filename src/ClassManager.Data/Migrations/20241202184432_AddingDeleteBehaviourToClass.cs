using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingDeleteBehaviourToClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ClassDays_ClassDayId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassDays_Classes_ClassId",
                table: "ClassDays");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClasses_Classes_ClassId",
                table: "StudentsClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClasses_Users_UserId",
                table: "StudentsClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachersClasses_Classes_ClassId",
                table: "TeachersClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachersClasses_Users_UserId",
                table: "TeachersClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ClassDays_ClassDayId",
                table: "Bookings",
                column: "ClassDayId",
                principalTable: "ClassDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDays_Classes_ClassId",
                table: "ClassDays",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClasses_Classes_ClassId",
                table: "StudentsClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClasses_Users_UserId",
                table: "StudentsClasses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersClasses_Classes_ClassId",
                table: "TeachersClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersClasses_Users_UserId",
                table: "TeachersClasses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ClassDays_ClassDayId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassDays_Classes_ClassId",
                table: "ClassDays");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClasses_Classes_ClassId",
                table: "StudentsClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClasses_Users_UserId",
                table: "StudentsClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachersClasses_Classes_ClassId",
                table: "TeachersClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachersClasses_Users_UserId",
                table: "TeachersClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ClassDays_ClassDayId",
                table: "Bookings",
                column: "ClassDayId",
                principalTable: "ClassDays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDays_Classes_ClassId",
                table: "ClassDays",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClasses_Classes_ClassId",
                table: "StudentsClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClasses_Users_UserId",
                table: "StudentsClasses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersClasses_Classes_ClassId",
                table: "TeachersClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersClasses_Users_UserId",
                table: "TeachersClasses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
