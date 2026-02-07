using FluentAssertions;
using School.Domain.Entities;
using Xunit;

namespace School.XUnitTest.Domain
{
    public class SubjectTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCollections()
        {
            // Arrange & Act
            var subject = new Subject();

            // Assert
            subject.StudentsSubjects.Should().NotBeNull();
            subject.StudentsSubjects.Should().BeEmpty();

            subject.DepartmetsSubjects.Should().NotBeNull();
            subject.DepartmetsSubjects.Should().BeEmpty();

            subject.InstructorSubjects.Should().NotBeNull();
            subject.InstructorSubjects.Should().BeEmpty();
        }

        [Fact]
        public void Properties_ShouldSetAndRetrieveValues()
        {
            // Arrange
            var subject = new Subject
            {
                Id = 1,
                SubjectNameAr = "SubjectNameAr",
                SubjectNameEn = "SubjectNameEn",
                Period = 2
            };

            // Assert
            subject.Id.Should().Be(1);
            subject.SubjectNameAr.Should().Be("SubjectNameAr");
            subject.SubjectNameEn.Should().Be("SubjectNameEn");
            subject.Period.Should().Be(2);
        }
    }
}
