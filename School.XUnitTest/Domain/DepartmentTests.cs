using FluentAssertions;
using School.Domain.Entities;
using Xunit;

namespace School.XUnitTest.Domain
{
    public class DepartmentTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCollections()
        {
            // Arrange & Act
            var department = new Department();

            // Assert
            department.Students.Should().NotBeNull();
            department.Students.Should().BeEmpty();

            department.DepartmentSubjects.Should().NotBeNull();
            department.DepartmentSubjects.Should().BeEmpty();

            department.Instructors.Should().NotBeNull();
            department.Instructors.Should().BeEmpty();
        }

        [Fact]
        public void Properties_ShouldSetAndRetrieveValues()
        {
            // Arrange
            var department = new Department
            {
                Id = 1,
                DepartmentNameAr = "DepNameAr",
                DepartmentNameEn = "DepNameEn",
                InstructorManagerId = 10
            };

            // Assert
            department.Id.Should().Be(1);
            department.DepartmentNameAr.Should().Be("DepNameAr");
            department.DepartmentNameEn.Should().Be("DepNameEn");
            department.InstructorManagerId.Should().Be(10);
        }
    }
}
