using School.Domain.Entities.Views;

public static class DepartmentTotalStudentViewFixture
{
    public static List<DepartementTotalStudentView> CreateList()
    {
        return new List<DepartementTotalStudentView>
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
