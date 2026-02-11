using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Autorazation.Queries.Handlers;
using School.Core.Features.Autorazation.Queries.Models;
using School.Core.Resources;
using School.Domain.Results;
using School.Service.Services.Interfaces;
using School.Tests.Helper;
using System.Net;

namespace School.Tests.Features.Autorazation.Query
{
    public class ClaimsQueryHandlerTests
    {
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<UserManager<Domain.Entities.Identity.User>> _userManagerMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;

        public ClaimsQueryHandlerTests()
        {
            _authorizationServiceMock = new();
            _userManagerMock = MockHelper.MockUserManager<Domain.Entities.Identity.User>();
            _localizerMock = new();
        }

        [Fact]
        public async Task Handle_ManageUserClaims_Should_Return_404_When_UserNotFound()
        {
            // Arrange
            var handler = new ClaimsQueryHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object,
                _userManagerMock.Object);

            _userManagerMock
                .Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync((Domain.Entities.Identity.User)null!);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.UserNotFound])
                .Returns(new LocalizedString(SharedResourcesKeys.UserNotFound, "User Not Found"));

            var query = new ManageUserClaimsQuery { UserId = 1 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("User Not Found");
        }

        [Fact]
        public async Task Handle_ManageUserClaims_Should_Return_200_When_Success()
        {
            // Arrange
            var handler = new ClaimsQueryHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object,
                _userManagerMock.Object);

            var user = new Domain.Entities.Identity.User { Id = 1, UserName = "test" };
            var expected = new ManageUserClaimsResult();

            _userManagerMock
                .Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _authorizationServiceMock
                .Setup(x => x.ManageUserClaimData(user))
                .ReturnsAsync(expected);

            var query = new ManageUserClaimsQuery { UserId = 1 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be(expected);

            _authorizationServiceMock.Verify(x => x.ManageUserClaimData(user), Times.Once);
        }
    }
}
