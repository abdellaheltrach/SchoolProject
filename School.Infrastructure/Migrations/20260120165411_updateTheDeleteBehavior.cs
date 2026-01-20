using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTheDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectId",
                table: "departmetSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_instructorSubjects_instructors_InstructorId",
                table: "instructorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_instructorSubjects_subjects_SubjectId",
                table: "instructorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_students_StudentId",
                table: "studentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_subjects_SubjectId",
                table: "studentSubjects");

            migrationBuilder.AddForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectId",
                table: "departmetSubjects",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_instructorSubjects_instructors_InstructorId",
                table: "instructorSubjects",
                column: "InstructorId",
                principalTable: "instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_instructorSubjects_subjects_SubjectId",
                table: "instructorSubjects",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentSubjects_students_StudentId",
                table: "studentSubjects",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentSubjects_subjects_SubjectId",
                table: "studentSubjects",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectId",
                table: "departmetSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_instructorSubjects_instructors_InstructorId",
                table: "instructorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_instructorSubjects_subjects_SubjectId",
                table: "instructorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_students_StudentId",
                table: "studentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_studentSubjects_subjects_SubjectId",
                table: "studentSubjects");

            migrationBuilder.AddForeignKey(
                name: "FK_departmetSubjects_subjects_SubjectId",
                table: "departmetSubjects",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
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
    }
}
