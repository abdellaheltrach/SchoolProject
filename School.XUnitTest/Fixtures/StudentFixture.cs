using School.Domain.Entities;

namespace School.XUnitTest.Fixtures
{
    public static class StudentFixture
    {
        public static Student CreateValidStudent(int id = 1, int departmentId = 1)
        {
            return new Student
            {
                StudentID = id,
                NameEn = "Test Student",
                NameAr = "طالب اختبار",
                Address = "123 Test St",
                Phone = "1234567890",
                DepartementId = departmentId
            };
        }

        public static List<Student> CreateStudentList(int count, int departmentId = 1)
        {
            var students = new List<Student>();
            for (int i = 1; i <= count; i++)
            {
                students.Add(new Student
                {
                    StudentID = i,
                    NameEn = $"Student {i}",
                    NameAr = $"طالب {i}",
                    Address = $"Address {i}",
                    Phone = $"Phone {i}",
                    DepartementId = departmentId
                });
            }
            return students;
        }
    }
}
