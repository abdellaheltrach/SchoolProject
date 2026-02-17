using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using School.Domain.Entities.Identity;
using School.Domain.Results;
using School.Domain.Results.Requests;
using School.Infrastructure.Bases.UnitOfWork;
using School.Service.Services;
using School.Tests.Fixtures;
using School.Tests.Helper;
using System.Security.Claims;

namespace School.Tests.Services
{
    public class AuthorizationServiceTests
    {
        private readonly Mock<RoleManager<Role>> _roleManagerMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AuthorizationService _authorizationService;

        public AuthorizationServiceTests()
        {
            _roleManagerMock = MockHelper.MockRoleManager<Role>();
            _userManagerMock = MockHelper.MockUserManager<User>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _authorizationService = new AuthorizationService(
                _roleManagerMock.Object,
                _userManagerMock.Object,
                _unitOfWorkMock.Object);
        }

        #region AddRoleAsync Tests
        [Fact]
        public async Task AddRoleAsync_ShouldReturnTrue_WhenSuccess()
        {
            // Arrange
            _roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authorizationService.AddRoleAsync("Admin");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AddRoleAsync_ShouldReturnFalse_WhenCreateFails()
        {
            // Arrange
            _roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _authorizationService.AddRoleAsync("Admin");

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region EditRoleAsync Tests
        [Fact]
        public async Task EditRoleAsync_ShouldReturnTrue_WhenSuccess()
        {
            // Arrange
            var role = RoleFixture.CreateRole(1, "OldRole");
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(role);
            _roleManagerMock.Setup(m => m.UpdateAsync(role))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authorizationService.EditRoleAsync(1, "NewRole");

            // Assert
            result.Should().BeTrue();
            role.Name.Should().Be("NewRole");
        }

        [Fact]
        public async Task EditRoleAsync_ShouldReturnFalse_WhenRoleNotFound()
        {
            // Arrange
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync((Role?)null);

            // Act
            var result = await _authorizationService.EditRoleAsync(1, "NewRole");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task EditRoleAsync_ShouldReturnFalse_WhenUpdateFails()
        {
            // Arrange
            var role = RoleFixture.CreateRole(1, "OldRole");
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(role);
            _roleManagerMock.Setup(m => m.UpdateAsync(role))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _authorizationService.EditRoleAsync(1, "NewRole");

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region DeleteRoleAsync Tests
        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnSuccess_WhenRoleNotUsed()
        {
            // Arrange
            var role = RoleFixture.CreateRole(1, "Admin");
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(role);
            _userManagerMock.Setup(m => m.GetUsersInRoleAsync("Admin"))
                .ReturnsAsync(new List<User>());
            _roleManagerMock.Setup(m => m.DeleteAsync(role))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authorizationService.DeleteRoleAsync(1);

            // Assert
            result.Should().Be("Success");
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnUsed_WhenRoleIsUsed()
        {
            // Arrange
            var role = RoleFixture.CreateRole(1, "Admin");
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(role);
            _userManagerMock.Setup(m => m.GetUsersInRoleAsync("Admin"))
                .ReturnsAsync(new List<User> { UserFixture.CreateUser() });

            // Act
            var result = await _authorizationService.DeleteRoleAsync(1);

            // Assert
            result.Should().Be("Used");
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync((Role?)null);

            // Act
            var result = await _authorizationService.DeleteRoleAsync(1);

            // Assert
            result.Should().Be("NotFound");
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnErrors_WhenDeleteFails()
        {
            // Arrange
            var role = RoleFixture.CreateRole(1, "Admin");
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(role);
            _userManagerMock.Setup(m => m.GetUsersInRoleAsync("Admin"))
                .ReturnsAsync(new List<User>());
            _roleManagerMock.Setup(m => m.DeleteAsync(role))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error Message" }));

            // Act
            var result = await _authorizationService.DeleteRoleAsync(1);

            // Assert
            result.Should().Contain("Error Message");
        }
        #endregion

        #region UpdateUserRoles Tests
        [Fact]
        public async Task UpdateUserRoles_ShouldReturnSuccess()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            var updatedRoles = new List<UserRoles> 
            { 
                new UserRoles { Name = "Admin", HasRole = true },
                new UserRoles { Name = "User", HasRole = false }
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddToRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserRoles(1, updatedRoles);

            // Assert
            result.Should().Be("Success");
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserRoles_ShouldReturnUserIsNull_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync((User?)null);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserRoles(1, new List<UserRoles>());

            // Assert
            result.Should().Be("UserIsNull");
        }

        [Fact]
        public async Task UpdateUserRoles_ShouldReturnFailedToRemoveOldRoles_WhenRemoveFails()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string>());
            _userManagerMock.Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed());
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserRoles(1, new List<UserRoles>());

            // Assert
            result.Should().Be("FailedToRemoveOldRoles");
        }

        [Fact]
        public async Task UpdateUserRoles_ShouldReturnFailedToAddNewRoles_WhenAddFails()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string>());
            _userManagerMock.Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddToRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed());
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserRoles(1, new List<UserRoles>());

            // Assert
            result.Should().Be("FailedToAddNewRoles");
        }

        [Fact]
        public async Task UpdateUserRoles_ShouldRollback_OnException()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ThrowsAsync(new Exception());
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserRoles(1, new List<UserRoles>());

            // Assert
            result.Should().Be("FailedToUpdateUserRoles");
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
        #endregion

        #region UpdateUserClaims Tests
        [Fact]
        public async Task UpdateUserClaims_ShouldReturnSuccess()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            var request = new UpdateUserClaimsRequest
            {
                UserId = 1,
                userClaims = new List<UserClaims>
                {
                    new UserClaims { Type = "Create", Value = true },
                    new UserClaims { Type = "Delete", Value = false }
                }
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
            _userManagerMock.Setup(m => m.RemoveClaimsAsync(user, It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddClaimsAsync(user, It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(IdentityResult.Success);

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserClaims(request);

            // Assert
            result.Should().Be("Success");
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserClaims_ShouldReturnUserIsNull_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync((User?)null);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserClaims(new UpdateUserClaimsRequest { UserId = 1 });

            // Assert
            result.Should().Be("UserIsNull");
        }

        [Fact]
        public async Task UpdateUserClaims_ShouldReturnFailedToRemoveOldClaims_WhenRemoveFails()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
            _userManagerMock.Setup(m => m.RemoveClaimsAsync(user, It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(IdentityResult.Failed());
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserClaims(new UpdateUserClaimsRequest { UserId = 1 });

            // Assert
            result.Should().Be("FailedToRemoveOldClaims");
        }

        [Fact]
        public async Task UpdateUserClaims_ShouldReturnFailedToAddNewClaims_WhenAddFails()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            var request = new UpdateUserClaimsRequest 
            { 
                UserId = 1, 
                userClaims = new List<UserClaims> { new UserClaims { Type = "Type", Value = true } } 
            };
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
            _userManagerMock.Setup(m => m.RemoveClaimsAsync(user, It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddClaimsAsync(user, It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(IdentityResult.Failed());
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserClaims(request);

            // Assert
            result.Should().Be("FailedToAddNewClaims");
        }

        [Fact]
        public async Task UpdateUserClaims_ShouldRollback_OnException()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ThrowsAsync(new Exception());
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authorizationService.UpdateUserClaims(new UpdateUserClaimsRequest { UserId = 1 });

            // Assert
            result.Should().Be("FailedToUpdateClaims");
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        }
        #endregion
        #region Other Method Tests
        [Fact]
        public async Task IsRoleExistByName_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            _roleManagerMock.Setup(m => m.RoleExistsAsync("Admin"))
                .ReturnsAsync(true);

            // Act
            var result = await _authorizationService.IsRoleExistByName("Admin");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsRoleExistById_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            var role = RoleFixture.CreateRole(1, "Admin");
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(role);

            // Act
            var result = await _authorizationService.IsRoleExistById(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetRolesList_ShouldReturnRoles()
        {
            // Arrange
            var roles = new List<Role> { RoleFixture.CreateRole(1, "Admin") }.BuildMock();
            _roleManagerMock.Setup(m => m.Roles).Returns(roles);

            // Act
            var result = await _authorizationService.GetRolesList();

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetRoleById_ShouldReturnRole()
        {
            // Arrange
            var role = RoleFixture.CreateRole(1, "Admin");
            _roleManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(role);

            // Act
            var result = await _authorizationService.GetRoleById(1);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Admin");
        }

        [Fact]
        public async Task ManageUserRolesData_ShouldReturnCorrectData()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            var roles = new List<Role> 
            { 
                RoleFixture.CreateRole(1, "Admin"),
                RoleFixture.CreateRole(2, "User")
            }.BuildMock();
            
            _roleManagerMock.Setup(m => m.Roles).Returns(roles);
            _userManagerMock.Setup(m => m.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
            _userManagerMock.Setup(m => m.IsInRoleAsync(user, "User")).ReturnsAsync(false);

            // Act
            var result = await _authorizationService.ManageUserRolesData(user);

            // Assert
            result.UserId.Should().Be(1);
            result.userRoles.Should().HaveCount(2);
            result.userRoles.First(r => r.Name == "Admin").HasRole.Should().BeTrue();
            result.userRoles.First(r => r.Name == "User").HasRole.Should().BeFalse();
        }

        [Fact]
        public async Task ManageUserClaimData_ShouldReturnCorrectData()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            var userClaims = new List<Claim> { new Claim("Create Student", "false") };
            _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(userClaims);

            // Act
            var result = await _authorizationService.ManageUserClaimData(user);

            // Assert
            result.UserId.Should().Be(1);
            result.userClaims.Should().NotBeEmpty();
            result.userClaims.First(c => c.Type == "Create Student").Value.Should().BeTrue();
            result.userClaims.First(c => c.Type == "Edit Student").Value.Should().BeFalse();
        }
        #endregion
    }
}
