using School.Domain.Entities;

namespace School.XUnitTest.Fixtures
{
    public static class StudentFixture
    {
        public static Student CreateValidStudent(Department department, int? id = null, string? nameEn = null, string? nameAr = null)
        {
            return new Student
            {
                StudentID = id ?? 1,
                NameEn = nameEn ?? "Test Student",
                NameAr = nameAr ?? "طالب اختبار",
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
                    StudentID = i,
                    NameEn = $"Student {i}",
                    NameAr = $"طالب {i}",
                    Address = $"Address {i}",
                    Phone = $"Phone {i}",
                    Department = department,
                    DepartementId = department.Id
                });
            }
            return students;
        }

        public static List<Student> CreateStudentListForFiltering(Department department)
        {
            return new List<Student>
            {
                new Student { StudentID = 1, NameEn = "Alice", NameAr = "أليس", Address = "London", Department = department, DepartementId = department.Id },
                new Student { StudentID = 2, NameEn = "Bob", NameAr = "بوب", Address = "Paris", Department = department, DepartementId = department.Id },
                new Student { StudentID = 3, NameEn = "Charlie", NameAr = "تشارلي", Address = "London", Department = department, DepartementId = department.Id }
            };
        }
    }
}
