using School.Domain.Entities;

namespace School.XUnitTest.Fixtures
{
    public static class StudentFixture
    {
        public static Student CreateValidStudent(Department department)
        {
            return new Student
            {
                NameEn = "Test Student",
                NameAr = "طالب اختبار",
                Address = "123 Test St",
                Phone = "1234567890",
                Department = department
            };
        }

        public static List<Student> CreateStudentList(int count, Department department)
        {
            var students = new List<Student>();
            for (int i = 1; i <= count; i++)
            {
                students.Add(new Student
                {
                    NameEn = $"Student {i}",
                    NameAr = $"طالب {i}",
                    Address = $"Address {i}",
                    Phone = $"Phone {i}",
                    Department = department
                });
            }
            return students;
        }
    }
}
