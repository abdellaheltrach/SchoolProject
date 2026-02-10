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

        #region AddUserAsync tests
        [Fact]
        public async Task AddUserAsync_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var password = "Password123!";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync((User)null);

            _userManagerMock
                .Setup(x => x.FindByNameAsync(user.UserName))
                .ReturnsAsync((User)null);

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
        }


        #endregion


    }
}
