using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingColmunNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_departments_DID",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_DID",
                table: "students");

            migrationBuilder.DropColumn(
                name: "DID",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "DNameEn",
                table: "departments",
                newName: "DepartmentNameEn");

            migrationBuilder.RenameColumn(
                name: "DNameAr",
                table: "departments",
                newName: "DepartmentNameAr");

            migrationBuilder.CreateIndex(
                name: "IX_students_DepartementID",
                table: "students",
                column: "DepartementID");

            migrationBuilder.AddForeignKey(
                name: "FK_students_departments_DepartementID",
                table: "students",
                column: "DepartementID",
                principalTable: "departments",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_departments_DepartementID",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_DepartementID",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "DepartmentNameEn",
                table: "departments",
                newName: "DNameEn");

            migrationBuilder.RenameColumn(
                name: "DepartmentNameAr",
                table: "departments",
                newName: "DNameAr");

            migrationBuilder.AddColumn<int>(
                name: "DID",
                table: "students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_DID",
                table: "students",
                column: "DID");

            migrationBuilder.AddForeignKey(
                name: "FK_students_departments_DID",
                table: "students",
                column: "DID",
                principalTable: "departments",
                principalColumn: "ID");
        }
    }
}
