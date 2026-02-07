using FluentAssertions;
using School.Domain.Entities;
using Xunit;

namespace School.XUnitTest.Domain
{
    public class InstructorTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCollections()
        {
            // Arrange & Act
            var instructor = new Instructor();

            // Assert
            instructor.InstructorSubjects.Should().NotBeNull();
            instructor.InstructorSubjects.Should().BeEmpty();

            instructor.Subordinates.Should().NotBeNull();
            instructor.Subordinates.Should().BeEmpty();
        }

        [Fact]
        public void Properties_ShouldSetAndRetrieveValues()
        {
            // Arrange
            var instructor = new Instructor
            {
                InstructorId = 1,
                InstructorNameAr = "NameAr",
                InstructorNameEn = "NameEn",
                Address = "Address",
                Position = "Position",
                SupervisorId = 2,
                Salary = 1000m,
                Image = "Image.png",
                DepartementId = 1
            };

            // Assert
            instructor.InstructorId.Should().Be(1);
            instructor.InstructorNameAr.Should().Be("NameAr");
            instructor.InstructorNameEn.Should().Be("NameEn");
            instructor.Address.Should().Be("Address");
            instructor.Position.Should().Be("Position");
            instructor.SupervisorId.Should().Be(2);
            instructor.Salary.Should().Be(1000m);
            instructor.Image.Should().Be("Image.png");
            instructor.DepartementId.Should().Be(1);
        }
    }
}
