using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeStudentColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudID",
                table: "students",
                newName: "StudentID");

            migrationBuilder.AddColumn<int>(
                name: "DepartementID",
                table: "students",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartementID",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "students",
                newName: "StudID");
        }
    }
}
