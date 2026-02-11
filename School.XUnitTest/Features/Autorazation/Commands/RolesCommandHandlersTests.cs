using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Autorazation.Commands.Handlers;
using School.Core.Features.Autorazation.Commands.Models;
using School.Core.Resources;
using School.Domain.Results;
using School.Service.Services.Interfaces;
using System.Net;

namespace School.Tests.Features.Autorazation.Commands
{
    public class RolesCommandHandlersTests
    {
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;

        public RolesCommandHandlersTests()
        {
            _authorizationServiceMock = new();
            _localizerMock = new();
        }
        #region AddRole Tests

        [Fact]
        public async Task Handle_AddRole_Should_Return_200_When_Success()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new AddRoleCommand { RoleName = "Admin" };

            _authorizationServiceMock
                .Setup(x => x.AddRoleAsync(command.RoleName))
                .ReturnsAsync(true);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            _authorizationServiceMock.Verify(
                x => x.AddRoleAsync(command.RoleName),
                Times.Once);
        }

        [Fact]
        public async Task Handle_AddRole_Should_Return_400_When_Failed()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new AddRoleCommand { RoleName = "Admin" };

            _authorizationServiceMock
                .Setup(x => x.AddRoleAsync(command.RoleName))
                .ReturnsAsync(false);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.CreateFailed])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.CreateFailed,
                    "Create Failed"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Create Failed");

            _authorizationServiceMock.Verify(
                x => x.AddRoleAsync(command.RoleName),
                Times.Once);
        }
        #endregion

        #region Edit Role Command Tests
        [Fact]
        public async Task Handle_EditRole_Should_Return_200_When_Success()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new EditRoleCommand
            {
                Id = 1,
                NewRoleName = "SuperAdmin"
            };

            _authorizationServiceMock
                .Setup(x => x.EditRoleAsync(command.Id, command.NewRoleName))
                .ReturnsAsync(true);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.Success])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.Success,
                    "Success"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("Success");

            _authorizationServiceMock.Verify(
                x => x.EditRoleAsync(command.Id, command.NewRoleName),
                Times.Once);
        }

        [Fact]
        public async Task Handle_EditRole_Should_Return_404_When_NotFound()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new EditRoleCommand
            {
                Id = 99,
                NewRoleName = "Unknown"
            };

            _authorizationServiceMock
                .Setup(x => x.EditRoleAsync(command.Id, command.NewRoleName))
                .ReturnsAsync(false);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _authorizationServiceMock.Verify(
                x => x.EditRoleAsync(command.Id, command.NewRoleName),
                Times.Once);
        }
        #endregion


        #region Delete Role Command Tests
        [Fact]
        public async Task Handle_DeleteRole_Should_Return_404_When_NotFound()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new DeleteRoleCommand(1);

            _authorizationServiceMock
                .Setup(x => x.DeleteRoleAsync(command.Id))
                .ReturnsAsync("NotFound");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            _authorizationServiceMock.Verify(
                x => x.DeleteRoleAsync(command.Id),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteRole_Should_Return_400_When_RoleIsUsed()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new DeleteRoleCommand(2);

            _authorizationServiceMock
                .Setup(x => x.DeleteRoleAsync(command.Id))
                .ReturnsAsync("Used");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.RoleIsUsed])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.RoleIsUsed,
                    "Role is used"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Role is used");
        }

        [Fact]
        public async Task Handle_DeleteRole_Should_Return_200_When_Success()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new DeleteRoleCommand(3);

            _authorizationServiceMock
                .Setup(x => x.DeleteRoleAsync(command.Id))
                .ReturnsAsync("Success");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.Deleted])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.Deleted,
                    "Deleted"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("Deleted");
        }

        [Fact]
        public async Task Handle_DeleteRole_Should_Return_400_With_Custom_Message_When_Other_Error()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = new DeleteRoleCommand(4);

            _authorizationServiceMock
                .Setup(x => x.DeleteRoleAsync(command.Id))
                .ReturnsAsync("Some custom error");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Some custom error");
        }
        #endregion

        #region update user roles command handler


        private UpdateUserRolesCommand CreateCommand(string userId = "1")
        {
            return new UpdateUserRolesCommand
            {
                UserId = int.Parse(userId),
                userRoles = new List<UserRoles>
                {
                    new UserRoles { Id = 1, Name = "Admin", HasRole = true }
                }
            };
        }

        [Fact]
        public async Task Handle_UpdateUserRoles_Should_Return_404_When_UserIsNull()
        {
            // Arrange
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserRoles(command.UserId, command.userRoles))
                .ReturnsAsync("UserIsNull");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.UserNotFound])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.UserNotFound,
                    "User not found"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task Handle_UpdateUserRoles_Should_Return_400_When_FailedToRemoveOldRoles()
        {
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserRoles(command.UserId, command.userRoles))
                .ReturnsAsync("FailedToRemoveOldRoles");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.FailedToRemoveOldRoles])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.FailedToRemoveOldRoles,
                    "Failed to remove old roles"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed to remove old roles");
        }

        [Fact]
        public async Task Handle_UpdateUserRoles_Should_Return_400_When_FailedToAddNewRoles()
        {
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserRoles(command.UserId, command.userRoles))
                .ReturnsAsync("FailedToAddNewRoles");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.FailedToAddNewRoles])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.FailedToAddNewRoles,
                    "Failed to add new roles"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed to add new roles");
        }

        [Fact]
        public async Task Handle_UpdateUserRoles_Should_Return_400_When_FailedToUpdateUserRoles()
        {
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserRoles(command.UserId, command.userRoles))
                .ReturnsAsync("FailedToUpdateUserRoles");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.FailedToUpdateUserRoles])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.FailedToUpdateUserRoles,
                    "Failed to update user roles"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed to update user roles");
        }

        [Fact]
        public async Task Handle_UpdateUserRoles_Should_Return_200_When_Success()
        {
            var handler = new RolesCommandHandlers(
                _authorizationServiceMock.Object,
                _localizerMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserRoles(command.UserId, command.userRoles))
                .ReturnsAsync("Success");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.Success])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.Success,
                    "Success"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("Success");
        }
        #endregion
    }
}
