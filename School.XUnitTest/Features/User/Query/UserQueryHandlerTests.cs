using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Features.Users.Queries.Handlers;
using School.Core.Features.Users.Queries.Models;
using School.Core.Features.Users.Queries.Response;
using School.Core.Mapping.UsersMapping;
using School.Core.Resources;
using School.Domain.Entities.Identity;
using School.Tests.Helper;
using System.Net;
using MockQueryable;
using MockQueryable.Moq;

namespace School.Tests.Features.User.Query
{
    public class UserQueryHandlerTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<School.Domain.Entities.Identity.User>> _userManagerMock;
        private readonly UserProfile _userProfile;

        public UserQueryHandlerTests()
        {
            _localizerMock = new();
            _userProfile = new();
            var configuration = new MapperConfiguration(c => c.AddProfile(_userProfile));
            _mapper = new Mapper(configuration);
            _userManagerMock = MockHelper.MockUserManager<School.Domain.Entities.Identity.User>();
        }

        #region Get User By Id Query
        [Fact]
        public async Task Handle_GetUserById_Should_Return_StatusCode200_When_Found()
        {
            // Arrange
            var handler = new UserQueryHandler(_localizerMock.Object, _userManagerMock.Object, _mapper);
            var query = new GetUserByIdQuery(1);
            var user = new School.Domain.Entities.Identity.User { Id = 1, FullName = "Test User" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.FullName.Should().Be("Test User");
            _userManagerMock.Verify(x => x.FindByIdAsync("1"), Times.Once);
        }

        [Fact]
        public async Task Handle_GetUserById_Should_Return_StatusCode404_When_NotFound()
        {
            // Arrange
            var handler = new UserQueryHandler(_localizerMock.Object, _userManagerMock.Object, _mapper);
            var query = new GetUserByIdQuery(1);
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync((School.Domain.Entities.Identity.User)null!);
            _localizerMock.Setup(l => l[SharedResourcesKeys.NotFound]).Returns(new LocalizedString(SharedResourcesKeys.NotFound, "Not Found"));

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("Not Found");
        }
        #endregion

        #region Get Paginated Users List Query
        [Fact]
        public async Task Handle_GetPaginatedUsersList_Should_Return_StatusCode200()
        {
            // Arrange
            var handler = new UserQueryHandler(_localizerMock.Object, _userManagerMock.Object, _mapper);
            var query = new GetPaginatedUsersListQuery { PageNumber = 1, PageSize = 10 };
            var users = new List<School.Domain.Entities.Identity.User> 
            { 
                new School.Domain.Entities.Identity.User { Id = 1, FullName = "User 1" } 
            }.BuildMock();

            _userManagerMock.Setup(x => x.Users).Returns(users);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Data.Should().HaveCount(1);
        }
        #endregion
    }
}
