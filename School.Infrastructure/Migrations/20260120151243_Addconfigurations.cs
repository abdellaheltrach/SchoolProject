using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addconfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departments_Instructor_InstructorManagerId",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "FK_departmetSubjects_departments_DepartementID",
                table: "departmetSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectID",
                table: "departmetSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Instructor_SupervisorId",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_departments_DepartementID",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorSubject_Instructor_InstructorId",
                table: "InstructorSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorSubject_subjects_SubjectId",
                table: "InstructorSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_students_departments_DepartementID",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_students_StudentID",
                table: "studentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_subjects_SubjectID",
                table: "studentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorSubject",
                table: "InstructorSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructor",
                table: "Instructor");

            migrationBuilder.RenameTable(
                name: "InstructorSubject",
                newName: "instructorSubjects");

            migrationBuilder.RenameTable(
                name: "Instructor",
                newName: "instructors");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "subjects",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "grade",
                table: "studentSubjects",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "SubjectID",
                table: "studentSubjects",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "studentSubjects",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_studentSubjects_SubjectID",
                table: "studentSubjects",
                newName: "IX_studentSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_studentSubjects_StudentID",
                table: "studentSubjects",
                newName: "IX_studentSubjects_StudentId");

            migrationBuilder.RenameColumn(
                name: "DepartementID",
                table: "students",
                newName: "DepartementId");

            migrationBuilder.RenameIndex(
                name: "IX_students_DepartementID",
                table: "students",
                newName: "IX_students_DepartementId");

            migrationBuilder.RenameColumn(
                name: "SubjectID",
                table: "departmetSubjects",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "DepartementID",
                table: "departmetSubjects",
                newName: "DepartementId");

            migrationBuilder.RenameIndex(
                name: "IX_departmetSubjects_SubjectID",
                table: "departmetSubjects",
                newName: "IX_departmetSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_departmetSubjects_DepartementID",
                table: "departmetSubjects",
                newName: "IX_departmetSubjects_DepartementId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "departments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Ins_SubId",
                table: "instructorSubjects",
                newName: "InsSubId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorSubject_SubjectId",
                table: "instructorSubjects",
                newName: "IX_instructorSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorSubject_InstructorId",
                table: "instructorSubjects",
                newName: "IX_instructorSubjects_InstructorId");

            migrationBuilder.RenameColumn(
                name: "DepartementID",
                table: "instructors",
                newName: "DepartementId");

            migrationBuilder.RenameIndex(
                name: "IX_Instructor_SupervisorId",
                table: "instructors",
                newName: "IX_instructors_SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_Instructor_DepartementID",
                table: "instructors",
                newName: "IX_instructors_DepartementId");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentNameAr",
                table: "departments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "instructors",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "instructors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InstructorNameEn",
                table: "instructors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InstructorNameAr",
                table: "instructors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "instructors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_instructorSubjects",
                table: "instructorSubjects",
                column: "InsSubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_instructors",
                table: "instructors",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_departments_instructors_InstructorManagerId",
                table: "departments",
                column: "InstructorManagerId",
                principalTable: "instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_departmetSubjects_departments_DepartementId",
                table: "departmetSubjects",
                column: "DepartementId",
                principalTable: "departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectId",
                table: "departmetSubjects",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_instructors_departments_DepartementId",
                table: "instructors",
                column: "DepartementId",
                principalTable: "departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_instructors_instructors_SupervisorId",
                table: "instructors",
                column: "SupervisorId",
                principalTable: "instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_instructorSubjects_instructors_InstructorId",
                table: "instructorSubjects",
                column: "InstructorId",
                principalTable: "instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_instructorSubjects_subjects_SubjectId",
                table: "instructorSubjects",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_students_departments_DepartementId",
                table: "students",
                column: "DepartementId",
                principalTable: "departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_studentSubjects_students_StudentId",
                table: "studentSubjects",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_studentSubjects_subjects_SubjectId",
                table: "studentSubjects",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departments_instructors_InstructorManagerId",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "FK_departmetSubjects_departments_DepartementId",
                table: "departmetSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectId",
                table: "departmetSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_instructors_departments_DepartementId",
                table: "instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_instructors_instructors_SupervisorId",
                table: "instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_instructorSubjects_instructors_InstructorId",
                table: "instructorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_instructorSubjects_subjects_SubjectId",
                table: "instructorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_students_departments_DepartementId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_students_StudentId",
                table: "studentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_subjects_SubjectId",
                table: "studentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_instructorSubjects",
                table: "instructorSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_instructors",
                table: "instructors");

            migrationBuilder.RenameTable(
                name: "instructorSubjects",
                newName: "InstructorSubject");

            migrationBuilder.RenameTable(
                name: "instructors",
                newName: "Instructor");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "subjects",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "studentSubjects",
                newName: "SubjectID");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "studentSubjects",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "studentSubjects",
                newName: "grade");

            migrationBuilder.RenameIndex(
                name: "IX_studentSubjects_SubjectId",
                table: "studentSubjects",
                newName: "IX_studentSubjects_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_studentSubjects_StudentId",
                table: "studentSubjects",
                newName: "IX_studentSubjects_StudentID");

            migrationBuilder.RenameColumn(
                name: "DepartementId",
                table: "students",
                newName: "DepartementID");

            migrationBuilder.RenameIndex(
                name: "IX_students_DepartementId",
                table: "students",
                newName: "IX_students_DepartementID");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "departmetSubjects",
                newName: "SubjectID");

            migrationBuilder.RenameColumn(
                name: "DepartementId",
                table: "departmetSubjects",
                newName: "DepartementID");

            migrationBuilder.RenameIndex(
                name: "IX_departmetSubjects_SubjectId",
                table: "departmetSubjects",
                newName: "IX_departmetSubjects_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_departmetSubjects_DepartementId",
                table: "departmetSubjects",
                newName: "IX_departmetSubjects_DepartementID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "departments",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "InsSubId",
                table: "InstructorSubject",
                newName: "Ins_SubId");

            migrationBuilder.RenameIndex(
                name: "IX_instructorSubjects_SubjectId",
                table: "InstructorSubject",
                newName: "IX_InstructorSubject_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_instructorSubjects_InstructorId",
                table: "InstructorSubject",
                newName: "IX_InstructorSubject_InstructorId");

            migrationBuilder.RenameColumn(
                name: "DepartementId",
                table: "Instructor",
                newName: "DepartementID");

            migrationBuilder.RenameIndex(
                name: "IX_instructors_SupervisorId",
                table: "Instructor",
                newName: "IX_Instructor_SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_instructors_DepartementId",
                table: "Instructor",
                newName: "IX_Instructor_DepartementID");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentNameAr",
                table: "departments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "Instructor",
                type: "decimal(6,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "Instructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InstructorNameEn",
                table: "Instructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InstructorNameAr",
                table: "Instructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Instructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorSubject",
                table: "InstructorSubject",
                column: "Ins_SubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructor",
                table: "Instructor",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_departments_Instructor_InstructorManagerId",
                table: "departments",
                column: "InstructorManagerId",
                principalTable: "Instructor",
                principalColumn: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_departmetSubjects_departments_DepartementID",
                table: "departmetSubjects",
                column: "DepartementID",
                principalTable: "departments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectID",
                table: "departmetSubjects",
                column: "SubjectID",
                principalTable: "subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Instructor_SupervisorId",
                table: "Instructor",
                column: "SupervisorId",
                principalTable: "Instructor",
                principalColumn: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_departments_DepartementID",
                table: "Instructor",
                column: "DepartementID",
                principalTable: "departments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorSubject_Instructor_InstructorId",
                table: "InstructorSubject",
                column: "InstructorId",
                principalTable: "Instructor",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorSubject_subjects_SubjectId",
                table: "InstructorSubject",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_departments_DepartementID",
                table: "students",
                column: "DepartementID",
                principalTable: "departments",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_studentSubjects_students_StudentID",
                table: "studentSubjects",
                column: "StudentID",
                principalTable: "students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentSubjects_subjects_SubjectID",
                table: "studentSubjects",
                column: "SubjectID",
                principalTable: "subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
