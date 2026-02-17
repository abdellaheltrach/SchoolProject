using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Autorazation.Queries.Handlers;
using School.Core.Features.Autorazation.Queries.Models;
using School.Core.Mapping.AutorazationRolesMapping;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Domain.Results;
using School.Service.Services.Interfaces;
using School.Tests.Helper;
using System.Net;

namespace School.Tests.Features.Autorazation.Query
{
    public class RolesQueryHandlerTests
    {
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<UserManager<Domain.Entities.Identity.User>> _userManagerMock;
        private readonly IMapper _mapper;

        public RolesQueryHandlerTests()
        {
            _authorizationServiceMock = new();
            _localizerMock = new();
            _userManagerMock = MockHelper.MockUserManager<Domain.Entities.Identity.User>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoleProfile>();
            });

            _mapper = config.CreateMapper();
        }

        #region GetRolesList

        [Fact]
        public async Task Handle_GetRolesList_Should_Return_200()
        {
            // Arrange
            var handler = new RolesQueryHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object,
                _mapper,
                _userManagerMock.Object);

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" }
            };

            _authorizationServiceMock
                .Setup(x => x.GetRolesList())
                .ReturnsAsync(roles);

            var query = new GetRolesListQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().HaveCount(1);
        }

        #endregion

        #region GetRoleById

        [Fact]
        public async Task Handle_GetRoleById_Should_Return_404_When_NotFound()
        {
            // Arrange
            var handler = new RolesQueryHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object,
                _mapper,
                _userManagerMock.Object);

            _authorizationServiceMock
                .Setup(x => x.GetRoleById(1))
                .ReturnsAsync((Role)null!);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.RoleNotExist])
                .Returns(new LocalizedString(SharedResourcesKeys.RoleNotExist, "Role Not Exist"));

            var query = new GetRoleByIdQuery() { Id = 1 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("Role Not Exist");
        }

        [Fact]
        public async Task Handle_GetRoleById_Should_Return_200_When_Found()
        {
            // Arrange
            var handler = new RolesQueryHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object,
                _mapper,
                _userManagerMock.Object);

            var role = new Role { Id = 1, Name = "Admin" };

            _authorizationServiceMock
                .Setup(x => x.GetRoleById(1))
                .ReturnsAsync(role);

            var query = new GetRoleByIdQuery() { Id = 1 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        #endregion

        #region ManageUserRoles

        [Fact]
        public async Task Handle_ManageUserRoles_Should_Return_404_When_UserNotFound()
        {
            // Arrange
            var handler = new RolesQueryHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object,
                _mapper,
                _userManagerMock.Object);

            _userManagerMock
                .Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync((Domain.Entities.Identity.User)null!);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.NotFound])
                .Returns(new LocalizedString(SharedResourcesKeys.NotFound, "Not Found"));

            var query = new ManageUserRolesQuery { UserId = 1 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("Not Found");
        }

        [Fact]
        public async Task Handle_ManageUserRoles_Should_Return_200_When_Success()
        {
            // Arrange
            var handler = new RolesQueryHandler(
                _localizerMock.Object,
                _authorizationServiceMock.Object,
                _mapper,
                _userManagerMock.Object);

            var user = new Domain.Entities.Identity.User { Id = 1, UserName = "test" };
            var manageResult = new ManageUserRolesResult();

            _userManagerMock
                .Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _authorizationServiceMock
                .Setup(x => x.ManageUserRolesData(user))
                .ReturnsAsync(manageResult);

            var query = new ManageUserRolesQuery { UserId = 1 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be(manageResult);
            _authorizationServiceMock.Verify(x => x.ManageUserRolesData(user), Times.Once);
        }

        #endregion
    }
}
