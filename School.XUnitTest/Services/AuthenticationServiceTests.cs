using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Infrastructure.Bases.GenericRepository;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Repositories.Interfaces;
using School.Service.Services;
using School.Service.Services.Interfaces;
using School.Tests.Fixtures;
using School.Tests.Helper;
using MockQueryable;
using System.Security.Claims;

namespace School.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IUserRefreshTokenRepository> _userRefreshTokenRepositoryMock;
        private readonly Mock<IEmailsService> _emailsServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly JwtSettings _jwtSettings;
        private readonly CookieSettings _cookieSettings;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _userManagerMock = MockHelper.MockUserManager<User>();
            _userRefreshTokenRepositoryMock = new Mock<IUserRefreshTokenRepository>();
            _emailsServiceMock = new Mock<IEmailsService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _jwtSettings = new JwtSettings
            {
                SecretKey = "super-secret-key-that-is-long-enough",
                Issuer = "School",
                Audience = "School",
                AccessTokenExpirationTimeInMinutes = 60,
                ValidateLifeTime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };

            _cookieSettings = new CookieSettings
            {
                RefreshTokenExpirationTimeInDays = 7
            };

            _authenticationService = new AuthenticationService(
                _jwtSettings,
                _cookieSettings,
                _userManagerMock.Object,
                _unitOfWorkMock.Object,
                _emailsServiceMock.Object,
                _userRefreshTokenRepositoryMock.Object);
        }

        #region GenerateJwtTokenAsync Tests
        [Fact]
        public async Task GenerateJwtTokenAsync_ShouldReturnTokens()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
            
            _userRefreshTokenRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync((UserRefreshToken)null!);
            _unitOfWorkMock.Setup(u => u.Repository<UserRefreshToken>()).Returns(new Mock<IGenericRepositoryAsync<UserRefreshToken>>().Object);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authenticationService.GenerateJwtTokenAsync(user);

            // Assert
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }
        #endregion

        #region RefreshJwtTokenAsync Tests
        [Fact]
        public async Task RefreshJwtTokenAsync_InvalidRefreshToken_ShouldReturnFalse()
        {
            // Arrange
            _userRefreshTokenRepositoryMock.Setup(r => r.GetTableAsTracking())
                .Returns(new List<UserRefreshToken>().BuildMock());

            // Act
            var result = await _authenticationService.RefreshJwtTokenAsync("access", "invalid-refresh");

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Invalid refresh token");
        }
        #endregion

        #region ValidateJwtToken Tests
        [Fact]
        public void ValidateJwtToken_EmptyToken_ShouldReturnFalse()
        {
            // Act
            var result = _authenticationService.ValidateJwtToken(string.Empty);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorMessage.Should().Contain("validation failed");
        }
        #endregion

        #region ConfirmEmail Tests
        [Fact]
        public async Task ConfirmEmail_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var user = UserFixture.CreateUser(1);
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.ConfirmEmailAsync(user, "code")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authenticationService.ConfirmEmail(1, "code");

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region SendResetPasswordCode Tests
        [Fact]
        public async Task SendResetPasswordCode_UserExists_ShouldReturnSuccess()
        {
            // Arrange
            var user = new User { Id = 1, Email = "test@test.com" };
            _userManagerMock.Setup(m => m.FindByEmailAsync("test@test.com")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authenticationService.SendResetPasswordCode("test@test.com");

            // Assert
            result.Should().Be("Success");
            user.Code.Should().NotBeNullOrEmpty();
            _emailsServiceMock.Verify(e => e.SendEmail(user.Email, It.IsAny<string>(), "Reset Password"), Times.Once);
        }
        #endregion

        #region ConfirmResetPassword Tests
        [Fact]
        public async Task ConfirmResetPassword_ValidCode_ShouldReturnSuccess()
        {
            // Arrange
            var user = new User { Email = "test@test.com", Code = "123456" };
            _userManagerMock.Setup(m => m.FindByEmailAsync("test@test.com")).ReturnsAsync(user);

            // Act
            var result = await _authenticationService.ConfirmResetPassword("123456", "test@test.com");

            // Assert
            result.Should().Be("Success");
        }

        [Fact]
        public async Task ConfirmResetPassword_InvalidCode_ShouldReturnFailed()
        {
            // Arrange
            var user = new User { Email = "test@test.com", Code = "123456" };
            _userManagerMock.Setup(m => m.FindByEmailAsync("test@test.com")).ReturnsAsync(user);

            // Act
            var result = await _authenticationService.ConfirmResetPassword("wrong", "test@test.com");

            // Assert
            result.Should().Be("Failed");
        }
        #endregion

        #region ResetPassword Tests

        [Fact]
        public async Task ResetPassword_ValidUser_ShouldReturnSuccess()
        {
            // Arrange
            var user = UserFixture.CreateUser(1, "test@test.com");
            _userManagerMock.Setup(m => m.FindByEmailAsync("test@test.com")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.HasPasswordAsync(user)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

            // Act
            var result = await _authenticationService.ResetPassword("test@test.com", "NewPassword123!");

            // Assert
            result.Should().Be("Success");
            _userManagerMock.Verify(m => m.RemovePasswordAsync(user), Times.Once);
            _userManagerMock.Verify(m => m.AddPasswordAsync(user, "NewPassword123!"), Times.Once);
        }
        #endregion
    }
}
