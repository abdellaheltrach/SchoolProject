using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Authentication.Queries.Handlers;
using School.Core.Features.Authentication.Queries.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;
using System.Net;

namespace School.Tests.Features.Authentication.Query
{
    public class AuthenticationQueryHandlerTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;

        public AuthenticationQueryHandlerTests()
        {
            _localizerMock = new();
            _authenticationServiceMock = new();
        }

        #region EmailConfirmation Tests

        [Fact]
        public async Task EmailConfirmation_Should_Return_400_When_NotConfirmed()
        {
            var handler = new AuthenticationQueryHandler(
                _localizerMock.Object,
                _authenticationServiceMock.Object);

            var query = new EmailConfirmationQuery
            {
                UserId = 1,
                code = "123"
            };

            _authenticationServiceMock
                .Setup(x => x.ConfirmEmail(query.UserId, query.code))
                .ReturnsAsync(false);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.ErrorWhenConfirmEmail])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.ErrorWhenConfirmEmail,
                    "Error"));

            var result = await handler.Handle(query, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task EmailConfirmation_Should_Return_200_When_Confirmed()
        {
            var handler = new AuthenticationQueryHandler(
                _localizerMock.Object,
                _authenticationServiceMock.Object);

            var query = new EmailConfirmationQuery
            {
                UserId = 1,
                code = "123"
            };

            _authenticationServiceMock
                .Setup(x => x.ConfirmEmail(query.UserId, query.code))
                .ReturnsAsync(true);

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.ConfirmEmailDone])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.ConfirmEmailDone,
                    "Confirmed"));

            var result = await handler.Handle(query, default);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("Confirmed");
        }

        #endregion

        #region ConfirmResetPassword Tests

        [Fact]
        public async Task ConfirmResetPassword_Should_Return_400_When_UserNotFound()
        {
            var handler = new AuthenticationQueryHandler(
                _localizerMock.Object,
                _authenticationServiceMock.Object);

            var query = new ConfirmResetPasswordQuery
            {
                Email = "test@mail.com",
                Code = "abc"
            };

            _authenticationServiceMock
                .Setup(x => x.ConfirmResetPassword(query.Code, query.Email))
                .ReturnsAsync("UserNotFound");

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.UserNotFound])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.UserNotFound,
                    "User not found"));

            var result = await handler.Handle(query, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task ConfirmResetPassword_Should_Return_400_When_Failed()
        {
            var handler = new AuthenticationQueryHandler(
                _localizerMock.Object,
                _authenticationServiceMock.Object);

            var query = new ConfirmResetPasswordQuery
            {
                Email = "test@mail.com",
                Code = "abc"
            };

            _authenticationServiceMock
                .Setup(x => x.ConfirmResetPassword(query.Code, query.Email))
                .ReturnsAsync("Failed");

            _localizerMock
                .Setup(x => x[SharedResourcesKeys.InvaildCode])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.InvaildCode,
                    "Invalid code"));

            var result = await handler.Handle(query, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Invalid code");
        }

        [Fact]
        public async Task ConfirmResetPassword_Should_Return_200_When_Success()
        {
            var handler = new AuthenticationQueryHandler(
                _localizerMock.Object,
                _authenticationServiceMock.Object);

            var query = new ConfirmResetPasswordQuery
            {
                Email = "test@mail.com",
                Code = "abc"
            };

            _authenticationServiceMock
                .Setup(x => x.ConfirmResetPassword(query.Code, query.Email))
                .ReturnsAsync("Success");

            var result = await handler.Handle(query, default);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion
    }
}
