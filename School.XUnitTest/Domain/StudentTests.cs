using FluentAssertions;
using School.Domain.Entities;
using Xunit;

namespace School.XUnitTest.Domain
{
    public class StudentTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCollections()
        {
            // Arrange & Act
            var student = new Student();

            // Assert
            student.StudentSubjects.Should().NotBeNull();
            student.StudentSubjects.Should().BeEmpty();
        }

        [Fact]
        public void Properties_ShouldSetAndRetrieveValues()
        {
            // Arrange
            var student = new Student
            {
                StudentID = 1,
                NameAr = "NameAr",
                NameEn = "NameEn",
                Address = "Address",
                Phone = "123456789",
                DepartementId = 1
            };

            // Assert
            student.StudentID.Should().Be(1);
            student.NameAr.Should().Be("NameAr");
            student.NameEn.Should().Be("NameEn");
            student.Address.Should().Be("Address");
            student.Phone.Should().Be("123456789");
            student.DepartementId.Should().Be(1);
        }
    }
}
