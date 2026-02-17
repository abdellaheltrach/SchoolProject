using School.Domain.Entities.Procedures;

namespace School.Tests.Fixtures.Procedure
{
    public class DepartmentStudentCountProcedureFixture
    {
        public static List<DepartmentStudentCountProcedure> CreateList()
        {
            return new()
        {
            new()
            {
                Id = 1,
                DepartmentNameAr = "علوم",
                DepartmentNameEn = "Science",
                TotalStudents = 10
            },
            new()
            {
                Id = 2,
                DepartmentNameAr = "آداب",
                DepartmentNameEn = "Arts",
                TotalStudents = 5
            }
        };
        }
    }
}
