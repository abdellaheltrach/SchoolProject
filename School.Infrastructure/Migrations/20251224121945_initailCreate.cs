using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initailCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsManager = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectNameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubjectNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    StudID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.StudID);
                    table.ForeignKey(
                        name: "FK_students_departments_DID",
                        column: x => x.DID,
                        principalTable: "departments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "departmetSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartementID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departmetSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_departmetSubjects_departments_DepartementID",
                        column: x => x.DepartementID,
                        principalTable: "departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_departmetSubjects_subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "subjects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "studentSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<int>(type: "int", nullable: false),
                    grade = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_studentSubjects_students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "students",
                        principalColumn: "StudID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_studentSubjects_subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "subjects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departmetSubjects_DepartementID",
                table: "departmetSubjects",
                column: "DepartementID");

            migrationBuilder.CreateIndex(
                name: "IX_departmetSubjects_SubjectID",
                table: "departmetSubjects",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_students_DID",
                table: "students",
                column: "DID");

            migrationBuilder.CreateIndex(
                name: "IX_studentSubjects_StudentID",
                table: "studentSubjects",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_studentSubjects_SubjectID",
                table: "studentSubjects",
                column: "SubjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "departmetSubjects");

            migrationBuilder.DropTable(
                name: "studentSubjects");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
