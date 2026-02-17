using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Base.ApiResponse;
using School.Core.Features.Users.Commands.Handlers;
using School.Core.Features.Users.Commands.Models;
using School.Core.Mapping.UsersMapping;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Service.Services.Interfaces;
using School.Tests.Helper;
using System.Net;
using MockQueryable;
using MockQueryable.Moq;

namespace School.Tests.Features.User.Command
{
    public class UserCommandHandlerTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<School.Domain.Entities.Identity.User>> _userManagerMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserProfile _userProfile;

        public UserCommandHandlerTests()
        {
            _localizerMock = new();
            _userProfile = new();
            var configuration = new MapperConfiguration(c => c.AddProfile(_userProfile));
            _mapper = new Mapper(configuration);
            _userManagerMock = MockHelper.MockUserManager<School.Domain.Entities.Identity.User>();
            _userServiceMock = new();
        }

        #region Add User Command
        [Fact]
        public async Task Handle_AddUser_Should_Return_StatusCode200_When_Success()
        {
            // Arrange
            var handler = new UserCommandHandler(_localizerMock.Object, _mapper, _userManagerMock.Object, _userServiceMock.Object);
            var command = new AddUserCommand { FullName = "Full Name", UserName = "username", Email = "email@test.com", Password = "Password123!", ConfirmationPassword = "Password123!" };
            _userServiceMock.Setup(x => x.AddUserAsync(It.IsAny<School.Domain.Entities.Identity.User>(), command.Password)).ReturnsAsync("Success");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            _userServiceMock.Verify(x => x.AddUserAsync(It.IsAny<School.Domain.Entities.Identity.User>(), command.Password), Times.Once);
        }

        [Fact]
        public async Task Handle_AddUser_Should_Return_StatusCode400_When_EmailExists()
        {
            // Arrange
            var handler = new UserCommandHandler(_localizerMock.Object, _mapper, _userManagerMock.Object, _userServiceMock.Object);
            var command = new AddUserCommand { Email = "email@test.com" };
            _userServiceMock.Setup(x => x.AddUserAsync(It.IsAny<School.Domain.Entities.Identity.User>(), It.IsAny<string>())).ReturnsAsync("EmailIsExist");
            _localizerMock.Setup(l => l[SharedResourcesKeys.EmailIsExist]).Returns(new LocalizedString(SharedResourcesKeys.EmailIsExist, "Email exists"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Email exists");
        }
        #endregion

        #region Edit User Command
        [Fact]
        public async Task Handle_EditUser_Should_Return_StatusCode404_When_UserNotFound()
        {
            // Arrange
            var handler = new UserCommandHandler(_localizerMock.Object, _mapper, _userManagerMock.Object, _userServiceMock.Object);
            var command = new EditUserCommand { Id = 1 };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync((School.Domain.Entities.Identity.User)null!);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _userManagerMock.Verify(x => x.FindByIdAsync("1"), Times.Once);
        }

        [Fact]
        public async Task Handle_EditUser_Should_Return_StatusCode200_When_Success()
        {
            // Arrange
            var handler = new UserCommandHandler(_localizerMock.Object, _mapper, _userManagerMock.Object, _userServiceMock.Object);
            var command = new EditUserCommand { Id = 1, FullName = "New Name", UserName = "newuser" };
            var user = new School.Domain.Entities.Identity.User { Id = 1, UserName = "olduser" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            
            // Mock empty users list for FirstOrDefaultAsync check
            var users = new List<School.Domain.Entities.Identity.User>().BuildMock();
            _userManagerMock.Setup(x => x.Users).Returns(users);

            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<School.Domain.Entities.Identity.User>())).ReturnsAsync(IdentityResult.Success);
            _localizerMock.Setup(l => l[SharedResourcesKeys.UserUpdated]).Returns(new LocalizedString(SharedResourcesKeys.UserUpdated, "User Updated"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("User Updated");
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<School.Domain.Entities.Identity.User>()), Times.Once);
        }
        #endregion

        #region Delete User Command
        [Fact]
        public async Task Handle_DeleteUser_Should_Return_StatusCode404_When_UserNotFound()
        {
            // Arrange
            var handler = new UserCommandHandler(_localizerMock.Object, _mapper, _userManagerMock.Object, _userServiceMock.Object);
            var command = new DeleteUserCommand(1);
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync((School.Domain.Entities.Identity.User)null!);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteUser_Should_Return_StatusCode200_When_Success()
        {
            // Arrange
            var handler = new UserCommandHandler(_localizerMock.Object, _mapper, _userManagerMock.Object, _userServiceMock.Object);
            var command = new DeleteUserCommand(1);
            var user = new School.Domain.Entities.Identity.User { Id = 1 };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
            _localizerMock.Setup(l => l[SharedResourcesKeys.Deleted]).Returns(new LocalizedString(SharedResourcesKeys.Deleted, "Deleted"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("Deleted");
            _userManagerMock.Verify(x => x.DeleteAsync(user), Times.Once);
        }
        #endregion

        #region Change User Password Command
        [Fact]
        public async Task Handle_ChangePassword_Should_Return_StatusCode200_When_Success()
        {
            // Arrange
            var handler = new UserCommandHandler(_localizerMock.Object, _mapper, _userManagerMock.Object, _userServiceMock.Object);
            var command = new ChangeUserPasswordCommand { Id = 1, CurrentPassword = "OldPassword!", NewPassword = "NewPassword!" };
            var user = new School.Domain.Entities.Identity.User { Id = 1 };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword)).ReturnsAsync(IdentityResult.Success);
            
            // In ChangeUserPasswordCommand handler, it returns Success<string>(_stringLocalizer[SharedResourcesKeys.Success])
            // This means Data is the localized "Success" string.
            _localizerMock.Setup(l => l[SharedResourcesKeys.Success]).Returns(new LocalizedString(SharedResourcesKeys.Success, "Success"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("Success");
            _userManagerMock.Verify(x => x.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword), Times.Once);
        }
        #endregion
    }
}
