using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using School.Domain.Entities.Identity;
using School.Domain.Helpers;
using School.Infrastructure.Bases.UnitOfWork;
using School.Service.Services;
using School.Service.Services.Interfaces;
using School.Tests.Fixtures;
using School.Tests.Helper;

namespace School.Tests.Services
{
    public class UserServiceTests
    {
        #region Fields
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IEmailsService> _emailsServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUrlHelper> _urlHelperMock;

        private readonly UserService _userServiceMock;
        #endregion
        #region Constructors
        public UserServiceTests()
        {
            _userManagerMock = MockHelper.MockUserManager<User>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _emailsServiceMock = new Mock<IEmailsService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _urlHelperMock = new Mock<IUrlHelper>();

            // Setup UnitOfWork transaction methods
            var mockTransaction = new Mock<IDbContextTransaction>();
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(mockTransaction.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            _userServiceMock = new UserService(_userManagerMock.Object,
                _httpContextAccessorMock.Object,
                _emailsServiceMock.Object,
                _unitOfWorkMock.Object,
                _urlHelperMock.Object);
        }
        #endregion

        #region AddUserAsync Tests
        [Fact]
        public async Task AddUserAsync_EmailExists_ReturnsEmailIsExist()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var password = "Password123!";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync(user);

            // Act
            var result = await _userServiceMock.AddUserAsync(user, password);

            // Assert
            result.Should().Be("EmailIsExist");
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_UserNameExists_ReturnsUserNameIsExist()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var password = "Password123!";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync((User?)null);

            _userManagerMock
                .Setup(x => x.FindByNameAsync(user.UserName!))
                .ReturnsAsync(user);

            // Act
            var result = await _userServiceMock.AddUserAsync(user, password);

            // Assert
            result.Should().Be("UserNameIsExist");
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_CreateFailed_ReturnsIdentityErrors()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var password = "Password123!";
            var errors = new List<IdentityError> { new IdentityError { Description = "Error 1" }, new IdentityError { Description = "Error 2" } };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync((User?)null);

            _userManagerMock
                .Setup(x => x.FindByNameAsync(user.UserName!))
                .ReturnsAsync((User?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(user, password))
                .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _userServiceMock.AddUserAsync(user, password);

            // Assert
            result.Should().Be("Error 1,Error 2");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_EmailSendFailed_ReturnsFailed()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var password = "Password123!";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync((User?)null);

            _userManagerMock
                .Setup(x => x.FindByNameAsync(user.UserName!))
                .ReturnsAsync((User?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(user, password))
                .ReturnsAsync(IdentityResult.Success);

            _emailsServiceMock
                .Setup(x => x.SendEmailConfirmationMail(user))
                .ReturnsAsync(false);

            // Act
            var result = await _userServiceMock.AddUserAsync(user, password);

            // Assert
            result.Should().Be("Failed");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var password = "Password123!";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync((User?)null);

            _userManagerMock
                .Setup(x => x.FindByNameAsync(user.UserName!))
                .ReturnsAsync((User?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(user, password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(user, AppRolesConstants.User))
                .ReturnsAsync(IdentityResult.Success);

            _emailsServiceMock
                .Setup(x => x.SendEmailConfirmationMail(user))
                .ReturnsAsync(true);

            // Act
            var result = await _userServiceMock.AddUserAsync(user, password);

            // Assert
            result.Should().Be("Success");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_Exception_ShouldRollbackAndReturnFailed()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var password = "Password123!";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email!))
                .ThrowsAsync(new Exception("Database Error"));

            // Act
            var result = await _userServiceMock.AddUserAsync(user, password);

            // Assert
            result.Should().Be("Failed");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
        #endregion


    }
}
