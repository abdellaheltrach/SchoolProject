using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Authentication.Commands.Handlers;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;
using School.Domain.Helpers;
using School.Domain.Options;
using School.Domain.Results;
using School.Service.Services.Interfaces;
using System.Net;
using System.Security.Claims;

namespace School.Tests.Features.Authentication.Command
{

    public class AuthenticationCommandHandlerTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<UserManager<Domain.Entities.Identity.User>> _userManagerMock;
        private readonly Mock<SignInManager<Domain.Entities.Identity.User>> _signInManagerMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        private readonly CookieSettings _cookieSettings;

        public AuthenticationCommandHandlerTests()
        {
            _localizerMock = new();

            var userStoreMock = new Mock<IUserStore<Domain.Entities.Identity.User>>();
            _userManagerMock = new Mock<UserManager<Domain.Entities.Identity.User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<Domain.Entities.Identity.User>>();

            _signInManagerMock = new Mock<SignInManager<Domain.Entities.Identity.User>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                null, null, null, null);

            _authenticationServiceMock = new();
            _authorizationServiceMock = new();
            _httpContextAccessorMock = new();

            _cookieSettings = new CookieSettings
            {
                HttpOnly = true,
                Secure = false,
                SameSite = "Lax",
                RefreshTokenExpirationTimeInDays = 7
            };
        }

        #region Helper Methods
        private DefaultHttpContext CreateHttpContextWithRefreshToken(string token)
        {
            var context = new DefaultHttpContext();

            var cookies = new Dictionary<string, string>
    {
        { "refreshToken", token }
    };

            context.Request.Headers["Cookie"] =
                string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}"));

            return context;
        }
        #endregion

        private AuthenticationCommandHandler CreateHandler()
        {
            return new AuthenticationCommandHandler(
                _localizerMock.Object,
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _authenticationServiceMock.Object,
                _cookieSettings,
                _httpContextAccessorMock.Object,
                _authorizationServiceMock.Object);
        }

        #region SignIn Tests
        [Fact]
        public async Task SignIn_Should_Return_400_When_User_NotFound()
        {
            var handler = CreateHandler();

            var command = new SignInCommand
            {
                Identifier = "test@test.com",
                Password = "123"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(command.Identifier))
                .ReturnsAsync((Domain.Entities.Identity.User)null);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.InvalidCredentials])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.InvalidCredentials,
                    "Invalid credentials"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Invalid credentials");
        }

        [Fact]
        public async Task SignIn_Should_Return_400_When_Password_Wrong()
        {
            var handler = CreateHandler();

            var user = new Domain.Entities.Identity.User
            {
                Id = 1,
                UserName = "test",
                EmailConfirmed = true
            };

            var command = new SignInCommand
            {
                Identifier = "test@test.com",
                Password = "wrong"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(command.Identifier))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(user, command.Password, false))
                .ReturnsAsync(SignInResult.Failed);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.InvalidCredentials])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.InvalidCredentials,
                    "Invalid credentials"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SignIn_Should_Return_400_When_Email_Not_Confirmed()
        {
            var handler = CreateHandler();

            var user = new Domain.Entities.Identity.User
            {
                Id = 1,
                UserName = "test",
                EmailConfirmed = false
            };

            var command = new SignInCommand
            {
                Identifier = "test@test.com",
                Password = "123"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(command.Identifier))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(user, command.Password, false))
                .ReturnsAsync(SignInResult.Success);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.EmailNotConfirmed])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.EmailNotConfirmed,
                    "Email not confirmed"));

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Email not confirmed");
        }

        [Fact]
        public async Task SignIn_Should_Return_200_And_Set_Cookie_When_Success()
        {
            var handler = CreateHandler();

            var user = new Domain.Entities.Identity.User
            {
                Id = 1,
                UserName = "test",
                EmailConfirmed = true
            };

            var command = new SignInCommand
            {
                Identifier = "test@test.com",
                Password = "123"
            };

            var tokenResult = new JwtAuthResult
            {
                AccessToken = "access",
                Succeeded = true,
                Message = "Success",
                RefreshToken = "refresh"

            };

            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(command.Identifier))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(user, command.Password, false))
                .ReturnsAsync(SignInResult.Success);

            _authenticationServiceMock
                .Setup(x => x.GenerateJwtTokenAsync(user))
                .ReturnsAsync(tokenResult);

            var result = await handler.Handle(command, default);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.AccessToken.Should().Be("access");

            httpContext.Response.Headers["Set-Cookie"]
                .ToString()
                .Should()
                .Contain("refreshToken");
        }
        #endregion

        #region Sign Out Tests
        [Fact]
        public async Task SignOut_Should_Return_400_When_User_Not_Authenticated()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // not authenticated

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.BadRequest])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.BadRequest,
                    "Bad request"));

            var handler = CreateHandler();

            var result = await handler.Handle(new SignOutCommand(), default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Bad request");
        }
        [Fact]
        public async Task SignOut_Should_Return_200_And_Delete_Cookie_When_Success()
        {
            var claims = new List<Claim>
    {
        new Claim(nameof(UserClaimModel.Id), "1"),
        new Claim(nameof(UserClaimModel.UserName), "test")
    };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.Success])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.Success,
                    "Success"));

            _signInManagerMock
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            var handler = CreateHandler();

            var result = await handler.Handle(new SignOutCommand(), default);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Message.Should().Be("Success");

            httpContext.Response.Headers["Set-Cookie"]
                .ToString()
                .Should()
                .Contain("refreshToken=");
        }
        [Fact]
        public async Task SignOut_Should_Return_400_When_Exception_Thrown()
        {
            var identity = new ClaimsIdentity(new[]
            {
        new Claim(nameof(UserClaimModel.Id), "1"),
        new Claim(nameof(UserClaimModel.UserName), "test")
    }, "TestAuthType");

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            };

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);

            _signInManagerMock
                .Setup(x => x.SignOutAsync())
                .ThrowsAsync(new Exception());

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.BadRequest])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.BadRequest,
                    "Bad request"));

            var handler = CreateHandler();

            var result = await handler.Handle(new SignOutCommand(), default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Refresh Token Tests
        [Fact]
        public async Task RefreshToken_Should_Return_BadRequest_When_Token_Still_Valid()
        {
            _authenticationServiceMock
                .Setup(x => x.ValidateJwtToken("access"))
                .Returns((true, null, string.Empty));

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.TokenStillValid])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.TokenStillValid,
                    "Token still valid"));

            var handler = CreateHandler();

            var result = await handler.Handle(
                new RefreshTokenCommand { AccessToken = "access" },
                default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Token still valid");
        }
        [Fact]
        public async Task RefreshToken_Should_Return_BadRequest_When_No_RefreshToken()
        {
            _authenticationServiceMock
                .Setup(x => x.ValidateJwtToken("access"))
                .Returns((false, null, "Token has expired"));

            var context = new DefaultHttpContext();

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(context);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.InvalidRefreshToken])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.InvalidRefreshToken,
                    "Invalid refresh token"));

            var handler = CreateHandler();

            var result = await handler.Handle(
                new RefreshTokenCommand { AccessToken = "access" },
                default);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshToken_Should_Return_BadRequest_When_Refresh_Fails()
        {
            _authenticationServiceMock
                .Setup(x => x.ValidateJwtToken("access"))
                .Returns((false, null, "Token has expired"));

            var context = CreateHttpContextWithRefreshToken("refresh");

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(context);

            _authenticationServiceMock
                .Setup(x => x.RefreshJwtTokenAsync("access", "refresh"))
                .ReturnsAsync(new JwtAuthResult
                {
                    Succeeded = false
                });

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.InvalidRefreshToken])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.InvalidRefreshToken,
                    "Invalid refresh token"));

            var handler = CreateHandler();

            var result = await handler.Handle(
                new RefreshTokenCommand { AccessToken = "access" },
                default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshToken_Should_Return_Success_When_Refresh_Succeeds()
        {
            _authenticationServiceMock
                .Setup(x => x.ValidateJwtToken("access"))
                .Returns((false, null, "Token has expired"));

            var context = CreateHttpContextWithRefreshToken("refresh");

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(context);

            _authenticationServiceMock
                .Setup(x => x.RefreshJwtTokenAsync("access", "refresh"))
                .ReturnsAsync(new JwtAuthResult
                {
                    Succeeded = true,
                    AccessToken = "newAccess",
                    RefreshToken = "newRefresh"
                });

            var handler = CreateHandler();

            var result = await handler.Handle(
                new RefreshTokenCommand { AccessToken = "access" },
                default);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.AccessToken.Should().Be("newAccess");
        }

        [Fact]
        public async Task RefreshToken_Should_Return_BadRequest_When_Token_Invalid()
        {
            _authenticationServiceMock
                .Setup(x => x.ValidateJwtToken("access"))
                .Returns((false, null, "Invalid signature"));

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.InvalidAccessToken])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.InvalidAccessToken,
                    "Invalid access token"));

            var handler = CreateHandler();

            var result = await handler.Handle(
                new RefreshTokenCommand { AccessToken = "access" },
                default);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        #endregion
    }
}
