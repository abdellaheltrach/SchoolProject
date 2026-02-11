using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Emails.Commands.Handlers;
using School.Core.Features.Emails.Commands.Models;
using School.Core.Resources;
using School.Service.Services.Interfaces;
using System.Net;

namespace School.Tests.Features.Emails
{
    public class EmailsCommandHandlerTests
    {
        private readonly Mock<IEmailsService> _emailsServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;

        public EmailsCommandHandlerTests()
        {
            _emailsServiceMock = new Mock<IEmailsService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();
        }

        #region Send Email Command

        [Fact]
        public async Task Handle_SendEmail_Should_Return_StatusCode200_When_Success()
        {
            // Arrange
            var handler = new EmailsCommandHandler(
                _localizerMock.Object,
                _emailsServiceMock.Object);

            var command = new SendEmailCommand
            {
                Email = "test@test.com",
                Message = "Test Message"
            };

            _emailsServiceMock
                .Setup(x => x.SendEmail(command.Email, command.Message, null))
                .ReturnsAsync(true);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("");

            _emailsServiceMock.Verify(
                x => x.SendEmail(command.Email, command.Message, null),
                Times.Once);
        }

        [Fact]
        public async Task Handle_SendEmail_Should_Return_StatusCode400_When_Failed()
        {
            // Arrange
            var handler = new EmailsCommandHandler(
                _localizerMock.Object,
                _emailsServiceMock.Object);

            var command = new SendEmailCommand
            {
                Email = "test@test.com",
                Message = "Test Message"
            };

            _emailsServiceMock
                .Setup(x => x.SendEmail(command.Email, command.Message, null))
                .ReturnsAsync(false);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.SendEmailFailed])
                .Returns(new LocalizedString(
                    SharedResourcesKeys.SendEmailFailed,
                    "Send email failed"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Send email failed");

            _emailsServiceMock.Verify(
                x => x.SendEmail(command.Email, command.Message, null),
                Times.Once);
        }

        #endregion
    }
}
