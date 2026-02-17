using School.Domain.Entities;
using School.Tests.Fixtures;

namespace School.XUnitTest.Fixtures
{
    public static class DepartmentFixture
    {
        public static Department CreateValidDepartment(string? nameEn = null)
        {
            return new Department
            {
                Id = 1,
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

        public static Department CreateValidDepartmentWithAllRelations(int id)
        {
            // create department
            var department = CreateValidDepartment();
            department.Id = id;

            // create instructors and assign to department
            var instructors = InstructorFixture.CreateInstructorList(3, department);
            department.Instructors = instructors;

            // assign the first instructor as the manager (navigation-based, NOT Id-based)
            department.InstructorManager = instructors.First();

            // create students and assign to department
            var students = StudentFixture.CreateStudentList(5, department);
            department.Students = students;

            // create subjects
            var subjects = SubjectFixture.CreateSubjectList(3);

            // create DepartmentSubject join entities
            department.DepartmentSubjects = subjects
                .Select(subject => new DepartmentSubject
                {
                    Department = department,
                    Subject = subject
                })
                .ToList();

            return department;
        }
    }
}
