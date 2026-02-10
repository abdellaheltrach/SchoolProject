using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using School.Domain.Entities;
using School.Domain.enums;
using School.Infrastructure.Bases.GenericRepository;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Reposetries.Interfaces;
using School.Service.Services;
using School.XUnitTest.Fixtures;

namespace School.Tests.Services
{
    public class StudentServiceTests
    {
        #region Fields
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepositoryAsync<Student>> _studentGenericRepoMock;
        private readonly StudentService _studentService;
        #endregion

        #region Constructors
        public StudentServiceTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _studentGenericRepoMock = new Mock<IGenericRepositoryAsync<Student>>();

            _studentService = new StudentService(_studentRepositoryMock.Object, _unitOfWorkMock.Object);
        }
        #endregion

        #region GetAllStudentListAsync Tests
        [Fact]
        public async Task GetAllStudentListAsync_ShouldReturnStudentList()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var students = StudentFixture.CreateStudentList(2, department);
            _studentRepositoryMock.Setup(r => r.GetAllStudentListAsync()).ReturnsAsync(students);

            // Act
            var result = await _studentService.GetAllStudentListAsync();

            // Assert
            result.Should().BeEquivalentTo(students);
            _studentRepositoryMock.Verify(r => r.GetAllStudentListAsync(), Times.Once);
        }
        #endregion

        #region GetStudentByIdWithNoTrachingAsync Tests
        [Fact]
        public async Task GetStudentByIdWithNoTrachingAsync_ShouldReturnStudent_WhenStudentExists()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            department.Id = 1;
            department.DepartmentNameEn = "Science";
            
            var students = StudentFixture.CreateStudentList(1, department);
            var mockQueryable = students.BuildMock();

            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(mockQueryable);

            // Act
            var result = await _studentService.GetStudentByIdWithNoTrachingAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.StudentID.Should().Be(1);
            result.Department.Should().NotBeNull();
            result.Department.DepartmentNameEn.Should().Be("Science");
        }

        [Fact]
        public async Task GetStudentByIdWithNoTrachingAsync_ShouldReturnNull_WhenStudentDoesNotExist()
        {
            // Arrange
            var students = new List<Student>().BuildMock();
            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(students);

            // Act
            var result = await _studentService.GetStudentByIdWithNoTrachingAsync(99);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetStudentByIdWithTrachingAsync Tests
        [Fact]
        public async Task GetStudentByIdWithTrachingAsync_ShouldReturnStudent_WhenStudentExists()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department, id: 1);
            _studentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);

            // Act
            var result = await _studentService.GetStudentByIdWithTrachingAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.StudentID.Should().Be(1);
        }
        #endregion

        #region AddStudentAsync Tests
        [Fact]
        public async Task AddStudentAsync_ShouldReturnSuccess_WhenStudentDoesNotExist()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department, nameEn: "New Student");

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);
            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(new List<Student>().BuildMock());
            _unitOfWorkMock.Setup(u => u.Repository<Student>()).Returns(_studentGenericRepoMock.Object);

            // Act
            var result = await _studentService.AddStudentAsync(student);

            // Assert
            result.success.Should().BeTrue();
            result.message.Should().Be("Student Added Successfully!");
            _studentGenericRepoMock.Verify(r => r.AddAsync(student), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task AddStudentAsync_ShouldReturnFailure_WhenStudentAlreadyExists()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department);
            var existingStudent = new List<Student> { student }.BuildMock();

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);
            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(existingStudent);

            // Act
            var result = await _studentService.AddStudentAsync(student);

            // Assert
            result.success.Should().BeFalse();
            result.message.Should().Be("The student already exists!");
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task AddStudentAsync_ShouldRollback_OnException()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department);

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);
            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Throws(new Exception());

            // Act
            var result = await _studentService.AddStudentAsync(student);

            // Assert
            result.success.Should().BeFalse();
            result.message.Should().Be("Failed to add student");
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
        #endregion

        #region EditAsync Tests
        [Fact]
        public async Task EditAsync_ShouldReturnSuccess_WhenUpdateSuccessful()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);
            _unitOfWorkMock.Setup(u => u.Repository<Student>()).Returns(_studentGenericRepoMock.Object);

            // Act
            var result = await _studentService.EditAsync(student);

            // Assert
            result.Should().Be("Success");
            _studentGenericRepoMock.Verify(r => r.UpdateAsync(student), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnFailed_OnException()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);
            _unitOfWorkMock.Setup(u => u.Repository<Student>()).Throws(new Exception());

            // Act
            var result = await _studentService.EditAsync(student);

            // Assert
            result.Should().Be("Failed");
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
        #endregion

        #region DeleteAsync Tests
        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleteSuccessful()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);
            _unitOfWorkMock.Setup(u => u.Repository<Student>()).Returns(_studentGenericRepoMock.Object);

            // Act
            var result = await _studentService.DeleteAsync(student);

            // Assert
            result.Should().BeTrue();
            _studentGenericRepoMock.Verify(r => r.DeleteAsync(student), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_OnException()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var student = StudentFixture.CreateValidStudent(department);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);
            _unitOfWorkMock.Setup(u => u.Repository<Student>()).Throws(new Exception());

            // Act
            var result = await _studentService.DeleteAsync(student);

            // Assert
            result.Should().BeFalse();
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
        #endregion

        #region Name Existence Tests
        [Fact]
        public async Task IsNameArExistExcludeSelf_ShouldReturnTrue_WhenNameExistsInOtherStudent()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var students = new List<Student> { StudentFixture.CreateValidStudent(department, id: 2, nameAr: "Existing") }.BuildMock();
            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(students);

            // Act
            var result = await _studentService.IsNameArExistExcludeSelf("Existing", 1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsNameArExistExcludeSelf_ShouldReturnFalse_WhenNameDoesNotExist()
        {
            // Arrange
            var students = new List<Student>().BuildMock();
            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(students);

            // Act
            var result = await _studentService.IsNameArExistExcludeSelf("NewName", 1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsNameEnExistExcludeSelf_ShouldReturnTrue_WhenNameExistsInOtherStudent()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var students = new List<Student> { StudentFixture.CreateValidStudent(department, id: 2, nameAr: "Existing") }.BuildMock(); // Note: Service code uses NameAr for both Ar and En methods
            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(students);

            // Act
            var result = await _studentService.IsNameEnExistExcludeSelf("Existing", 1);

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region FilterStudentPaginatedQuerable Tests
        [Fact]
        public void FilterStudentPaginatedQuerable_ShouldReturnFilteredList_WhenSearchProvided()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var students = StudentFixture.CreateStudentListForFiltering(department).BuildMock();

            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(students);

            // Act
            var result = _studentService.FilterStudentPaginatedQuerable("Alice", StudentOrdringEnum.NameEn, false).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].NameEn.Should().Be("Alice");
        }

        [Theory]
        [InlineData(StudentOrdringEnum.NameEn, false, "Alice")]
        [InlineData(StudentOrdringEnum.NameEn, true, "Charlie")]
        public void FilterStudentPaginatedQuerable_ShouldReturnOrderedList(StudentOrdringEnum ordering, bool sortDesc, string expectedFirst)
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var students = StudentFixture.CreateStudentListForFiltering(department).BuildMock();

            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(students);

            // Act
            var result = _studentService.FilterStudentPaginatedQuerable(null, ordering, sortDesc).ToList();

            // Assert
            result[0].NameEn.Should().Be(expectedFirst);
        }
        #endregion

        #region GetStudentsByDepartmentIDQuerable Tests
        [Fact]
        public void GetStudentsByDepartmentIDQuerable_ShouldReturnStudentsOfDepartment()
        {
            // Arrange
            var department1 = DepartmentFixture.CreateValidDepartment();
            department1.Id = 1;
            var department2 = DepartmentFixture.CreateValidDepartment();
            department2.Id = 2;
            
            var students = new List<Student>
            {
                StudentFixture.CreateValidStudent(department1, id: 1),
                StudentFixture.CreateValidStudent(department2, id: 2)
            }.BuildMock();
            
            // Set DepartementId explicitly just in case fixture didn't catch it
            students.First().DepartementId = 1;

            _studentRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(students);

            // Act
            var result = _studentService.GetStudentsByDepartmentIDQuerable(1).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].DepartementId.Should().Be(1);
        }
        #endregion
    }
}
