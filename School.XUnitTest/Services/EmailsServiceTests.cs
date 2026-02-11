using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Service.Services;
using School.Service.Services.Interfaces;
using School.Tests.Fixtures;
using School.Tests.Helper;
using System.Security.Claims;

namespace School.Tests.Services
{
    public class EmailsServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly EmailSettings _emailSettings;
        private readonly EmailsService _emailsService;

        public EmailsServiceTests()
        {
            _userManagerMock = MockHelper.MockUserManager<User>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _emailSenderMock = new Mock<IEmailSender>();
            
            _emailSettings = new EmailSettings
            {
                Host = "smtp.test.com",
                Port = 587,
                FromEmail = "test@school.com",
                Password = "password"
            };

            _emailsService = new EmailsService(
                _emailSettings,
                _userManagerMock.Object,
                _httpContextAccessorMock.Object,
                _urlHelperMock.Object,
                _emailSenderMock.Object);
        }

        #region SendEmailConfirmationMail Tests
        [Fact]
        public async Task SendEmailConfirmationMail_ShouldCallSendEmail_WithCorrectLink()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            var token = "confirmation-token";
            
            _userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(user))
                .ReturnsAsync(token);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost", 7124);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _urlHelperMock.Setup(h => h.Action(It.IsAny<UrlActionContext>()))
                .Returns("/confirm-email");

            _emailSenderMock.Setup(s => s.SendAsync(It.IsAny<MimeMessage>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _emailsService.SendEmailConfirmationMail(user);

            // Assert
            result.Should().BeTrue();
            
            _userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsync(user), Times.Once);
            _urlHelperMock.Verify(h => h.Action(It.Is<UrlActionContext>(c => 
                c.Action == "ConfirmEmail" && 
                c.Controller == "Authentication"
            )), Times.Once);

            _emailSenderMock.Verify(s => s.SendAsync(It.Is<MimeMessage>(m => 
                m.To.ToString().Contains(user.Email) && 
                m.Subject == "Email Confirmation"
            )), Times.Once);
        }

        [Fact]
        public async Task SendEmailConfirmationMail_OnException_ShouldReturnFalse()
        {
            // Arrange
            var user = UserFixture.CreateUser();
            _userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(user))
                .ThrowsAsync(new Exception("UserManager Error"));

            // Act
            var result = await _emailsService.SendEmailConfirmationMail(user);

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region SendEmail Tests
        [Fact]
        public async Task SendEmail_ShouldReturnTrue_WhenEmailIsSent()
        {
            // Arrange
            _emailSenderMock.Setup(s => s.SendAsync(It.IsAny<MimeMessage>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _emailsService.SendEmail("test@test.com", "body", "reason");

            // Assert
            result.Should().BeTrue();
            _emailSenderMock.Verify(s => s.SendAsync(It.Is<MimeMessage>(m => 
                m.To.ToString().Contains("test@test.com") && 
                m.Subject == "reason"
            )), Times.Once);
        }

        [Fact]
        public async Task SendEmail_ShouldReturnFalse_WhenSendingFails()
        {
            // Arrange
            _emailSenderMock.Setup(s => s.SendAsync(It.IsAny<MimeMessage>()))
                .ThrowsAsync(new Exception("SMTP Error"));

            // Act
            var result = await _emailsService.SendEmail("test@test.com", "body", "reason");

            // Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
