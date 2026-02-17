using School.Domain.Entities;

namespace School.Tests.Fixtures
{
    public static class InstructorFixture
    {
        public static Instructor CreateValidInstructor(
            Department department,
            int? id = null,
            string? nameEn = null,
            string? nameAr = null,
            Instructor? supervisor = null)
        {
            return new Instructor
            {
                InstructorId = id ?? 1,
                InstructorNameEn = nameEn ?? "Test Instructor",
                InstructorNameAr = nameAr ?? "مدرس اختبار",
                Address = "Instructor Address",
                Position = "Lecturer",
                Salary = 5000,
                Image = "image.png",

                Department = department,     // FK-safe
                Supervisor = supervisor      // optional self-reference
            };
        }

        public static List<Instructor> CreateInstructorList(
            int count,
            Department department)
        {
            var instructors = new List<Instructor>();

            for (int i = 1; i <= count; i++)
            {
                instructors.Add(new Instructor
                {
                    InstructorNameEn = $"Instructor {i}",
                    InstructorNameAr = $"مدرس {i}",
                    Address = $"Address {i}",
                    Position = "Lecturer",
                    Salary = 4000 + i * 500,
                    Image = $"img{i}.png",
                    Department = department
                });
            }

            return instructors;
        }

        public static Instructor CreateInstructorWithManagedDepartment(
            Department department,
            Department managedDepartment)
        {
            return new Instructor
            {
                InstructorNameEn = "Department Manager",
                InstructorNameAr = "رئيس قسم",
                Position = "Head of Department",
                Salary = 8000,

                Department = department,
                ManagedDepartment = managedDepartment
            };
        }
    }

}
