using School.Domain.Entities;

namespace School.XUnitTest.Fixtures
{
    public static class DepartmentFixture
    {
        public static Department CreateValidDepartment(string? nameEn = null)
        {
            return new Department
            {
                DepartmentNameEn = nameEn ?? "Computer Science",
                DepartmentNameAr = "علوم الحاسوب",
                InstructorManagerId = null
            };
        }

        public static List<Department> CreateDepartmentList(int count)
        {
            var departments = new List<Department>();
            for (int i = 1; i <= count; i++)
            {
                departments.Add(new Department
                {
                    DepartmentNameEn = $"Department {i}",
                    DepartmentNameAr = $"قسم {i}",
                    InstructorManagerId = null
                });
            }
            return departments;
        }
    }
}
