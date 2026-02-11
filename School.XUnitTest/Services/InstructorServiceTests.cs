using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using School.Domain.Entities;
using School.Infrastructure.Bases.GenericRepository;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Repositories.Interfaces;
using School.Infrastructure.Repositories.Interfaces.Functions;
using School.Service.Services;
using School.Service.Services._Interfaces;
using School.Service.Services.Interfaces;
using School.Tests.Fixtures;
using School.XUnitTest.Fixtures;

namespace School.Tests.Services
{
    public class InstructorServiceTests
    {
        #region Fields
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IInstructorRepository> _instructorsRepositoryMock;
        private readonly Mock<IInstructorFunctionsRepository> _instructorFunctionsRepositoryMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        private readonly InstructorService _instructorService;
        #endregion

        #region Constructors
        public InstructorServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _instructorsRepositoryMock = new Mock<IInstructorRepository>();
            _instructorFunctionsRepositoryMock = new Mock<IInstructorFunctionsRepository>();
            _fileServiceMock = new Mock<IFileService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Mock transaction
            var mockTransaction = new Mock<IDbContextTransaction>();
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(mockTransaction.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            _instructorService = new InstructorService(
                _unitOfWorkMock.Object,
                _instructorsRepositoryMock.Object,
                _instructorFunctionsRepositoryMock.Object,
                _fileServiceMock.Object,
                _httpContextAccessorMock.Object);
        }
        #endregion

        #region GetSalarySummationOfInstructor Tests
        [Fact]
        public async Task GetSalarySummationOfInstructor_ShouldReturnSummation()
        {
            // Arrange
            decimal expectedSalary = 5000;
            _instructorFunctionsRepositoryMock
                .Setup(r => r.GetSalarySummationOfInstructor(It.IsAny<string>()))
                .Returns(expectedSalary);

            // Act
            var result = await _instructorService.GetSalarySummationOfInstructor();

            // Assert
            result.Should().Be(expectedSalary);
            _instructorFunctionsRepositoryMock.Verify(r => r.GetSalarySummationOfInstructor(It.IsAny<string>()), Times.Once);
        }
        #endregion

        #region IsNameArExist Tests
        [Fact]
        public async Task IsNameArExist_ShouldReturnTrue_WhenNameExists()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructors = new List<Instructor> { InstructorFixture.CreateValidInstructor(department, nameAr: "مدرس") }.BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameArExist("مدرس");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsNameArExist_ShouldReturnFalse_WhenNameDoesNotExist()
        {
            // Arrange
            var instructors = new List<Instructor>().BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameArExist("مدرس");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsNameArExistExcludeSelf_ShouldReturnFalse_WhenNameDoesNotExist()
        {
            // Arrange
            var instructors = new List<Instructor>().BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameArExistExcludeSelf("مدرس", 1);

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region IsNameArExistExcludeSelf Tests
        [Fact]
        public async Task IsNameArExistExcludeSelf_ShouldReturnTrue_WhenNameExistsInOther()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructors = new List<Instructor> { InstructorFixture.CreateValidInstructor(department, id: 2, nameAr: "مدرس") }.BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameArExistExcludeSelf("مدرس", 1);

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region IsNameEnExist Tests
        [Fact]
        public async Task IsNameEnExist_ShouldReturnTrue_WhenNameExists()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructors = new List<Instructor> { InstructorFixture.CreateValidInstructor(department, nameEn: "Instructor") }.BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameEnExist("Instructor");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsNameEnExist_ShouldReturnFalse_WhenNameDoesNotExist()
        {
            // Arrange
            var instructors = new List<Instructor>().BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameEnExist("Instructor");

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region IsNameEnExistExcludeSelf Tests
        [Fact]
        public async Task IsNameEnExistExcludeSelf_ShouldReturnTrue_WhenNameExistsInOther()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructors = new List<Instructor> { InstructorFixture.CreateValidInstructor(department, id: 2, nameEn: "Instructor") }.BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameEnExistExcludeSelf("Instructor", 1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsNameEnExistExcludeSelf_ShouldReturnFalse_WhenNameDoesNotExist()
        {
            // Arrange
            var instructors = new List<Instructor>().BuildMock();
            _instructorsRepositoryMock.Setup(r => r.GetTableNoTracking()).Returns(instructors);

            // Act
            var result = await _instructorService.IsNameEnExistExcludeSelf("Instructor", 1);

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region AddInstructorAsync Tests
        [Fact]
        public async Task AddInstructorAsync_ShouldReturnSuccess_WhenAddSuccessful()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructor = InstructorFixture.CreateValidInstructor(department, id: 1);
            var fileMock = new Mock<IFormFile>();
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost");
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _fileServiceMock.Setup(s => s.UploadImage(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync("/image.jpg");
            
            var instructorRepoMock = new Mock<IGenericRepositoryAsync<Instructor>>();
            _unitOfWorkMock.Setup(u => u.Repository<Instructor>()).Returns(instructorRepoMock.Object);

            // Act
            var result = await _instructorService.AddInstructorAsync(instructor, fileMock.Object);

            // Assert
            result.Should().Be("Success");
            instructor.Image.Should().Be("http://localhost/image.jpg");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            instructorRepoMock.Verify(r => r.AddAsync(instructor), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task AddInstructorAsync_ShouldReturnNoImage_WhenUploadReturnsNoImage()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructor = InstructorFixture.CreateValidInstructor(department);
            var fileMock = new Mock<IFormFile>();
            
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _fileServiceMock.Setup(s => s.UploadImage(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync("NoImage");

            // Act
            var result = await _instructorService.AddInstructorAsync(instructor, fileMock.Object);

            // Assert
            result.Should().Be("NoImage");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task AddInstructorAsync_ShouldReturnFailedToUploadImage_WhenUploadReturnsFailed()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructor = InstructorFixture.CreateValidInstructor(department);
            var fileMock = new Mock<IFormFile>();
            
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _fileServiceMock.Setup(s => s.UploadImage(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync("FailedToUploadImage");

            // Act
            var result = await _instructorService.AddInstructorAsync(instructor, fileMock.Object);

            // Assert
            result.Should().Be("FailedToUploadImage");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task AddInstructorAsync_ShouldReturnFailedInAdd_OnException()
        {
            // Arrange
            var department = DepartmentFixture.CreateValidDepartment();
            var instructor = InstructorFixture.CreateValidInstructor(department);
            var fileMock = new Mock<IFormFile>();
            
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _fileServiceMock.Setup(s => s.UploadImage(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync("/image.jpg");
            _unitOfWorkMock.Setup(u => u.Repository<Instructor>()).Throws(new Exception());

            // Act
            var result = await _instructorService.AddInstructorAsync(instructor, fileMock.Object);

            // Assert
            result.Should().Be("FailedInAdd");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
        #endregion
    }
}
