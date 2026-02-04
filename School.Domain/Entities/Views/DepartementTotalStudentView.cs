using Microsoft.EntityFrameworkCore;

namespace School.Domain.Entities.Views
{
    /*
      create view DepartementTotalStudentView
as
SELECT departments.Id, departments.DepartmentNameAr, departments.DepartmentNameEn, count( students.StudentID) as TotalStudents
FROM     departments INNER JOIN
                  students ON departments.Id = students.DepartementId
				  group by  departments.Id, departments.DepartmentNameAr, departments.DepartmentNameEn
     */
    [Keyless]
    public class DepartementTotalStudentView
    {
        public int Id { get; set; }
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; }
        public int TotalStudents { get; set; }
    }
}
