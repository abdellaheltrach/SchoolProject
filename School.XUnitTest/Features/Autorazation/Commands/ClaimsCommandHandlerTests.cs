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
    public class ClaimsCommandHandlerTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;

        public ClaimsCommandHandlerTests()
        {
            _localizerMock = new();
            _authorizationServiceMock = new();
        }

        private UpdateUserClaimsCommand CreateCommand()
        {
            return new UpdateUserClaimsCommand
            {
                UserId = 1,
                userClaims = new List<UserClaims>
                {
                    new UserClaims
                    {
                        Type = "Permission",
                        Value = true
                    }
                }
            };
        }

        [Fact]
        public async Task Handle_Should_Return_404_When_UserIsNull()
        {
            var handler = new ClaimsCommandHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserClaims(command))
                .ReturnsAsync("UserIsNull");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.UserNotFound])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.UserNotFound,
                    "User not found"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task Handle_Should_Return_400_When_FailedToRemoveOldClaims()
        {
            var handler = new ClaimsCommandHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserClaims(command))
                .ReturnsAsync("FailedToRemoveOldClaims");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.FailedToRemoveOldClaims])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.FailedToRemoveOldClaims,
                    "Failed to remove old claims"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed to remove old claims");
        }

        [Fact]
        public async Task Handle_Should_Return_400_When_FailedToAddNewClaims()
        {
            var handler = new ClaimsCommandHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserClaims(command))
                .ReturnsAsync("FailedToAddNewClaims");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.FailedToAddNewClaims])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.FailedToAddNewClaims,
                    "Failed to add new claims"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed to add new claims");
        }

        [Fact]
        public async Task Handle_Should_Return_400_When_FailedToUpdateClaims()
        {
            var handler = new ClaimsCommandHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserClaims(command))
                .ReturnsAsync("FailedToUpdateClaims");

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.FailedToUpdateClaims])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.FailedToUpdateClaims,
                    "Failed to update claims"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed to update claims");
        }

        [Fact]
        public async Task Handle_Should_Return_200_When_Success()
        {
            var handler = new ClaimsCommandHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object);

            var command = CreateCommand();

            _authorizationServiceMock
                .Setup(x => x.UpdateUserClaims(command))
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
    }
}
