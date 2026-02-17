using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Base.ApiResponse;
using School.Core.Features.Instructors.Commands.Handlers;
using School.Core.Features.Instructors.Commands.Models;
using School.Core.Mapping.InstructorMapping;
using School.Core.Resources;
using School.Domain.Entities;
using School.Service.Services.Interfaces;
using System.Net;

namespace School.Tests.Features.Instructor.Command
{
    public class InstructorCommandHandlerTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly IMapper _mapper;
        private readonly Mock<IInstructorService> _instructorServiceMock;
        private readonly InstructorProfile _instructorProfile;

        public InstructorCommandHandlerTests()
        {
            _localizerMock = new();
            _instructorProfile = new();
            var configuration = new MapperConfiguration(c => c.AddProfile(_instructorProfile));
            _mapper = new Mapper(configuration);
            _instructorServiceMock = new();
        }

        #region Add Instructor Command
        [Fact]
        public async Task Handle_AddInstructor_Should_Return_StatusCode200_When_Success()
        {
            // Arrange
            var handler = new InstructorCommandHandler(_localizerMock.Object, _mapper, _instructorServiceMock.Object);
            var command = new AddInstructorCommand { InstructorNameAr = "اسم", InstructorNameEn = "Name", Address = "Address" };
            _instructorServiceMock.Setup(x => x.AddInstructorAsync(It.IsAny<School.Domain.Entities.Instructor>(), command.Image)).ReturnsAsync("Success");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            _instructorServiceMock.Verify(x => x.AddInstructorAsync(It.IsAny<School.Domain.Entities.Instructor>(), command.Image), Times.Once);
        }

        [Fact]
        public async Task Handle_AddInstructor_Should_Return_StatusCode400_When_NoImage()
        {
            // Arrange
            var handler = new InstructorCommandHandler(_localizerMock.Object, _mapper, _instructorServiceMock.Object);
            var command = new AddInstructorCommand();
            _instructorServiceMock.Setup(x => x.AddInstructorAsync(It.IsAny<School.Domain.Entities.Instructor>(), It.IsAny<Microsoft.AspNetCore.Http.IFormFile>())).ReturnsAsync("NoImage");
            _localizerMock.Setup(l => l[SharedResourcesKeys.NoImage]).Returns(new LocalizedString(SharedResourcesKeys.NoImage, "No Image"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("No Image");
        }

        [Fact]
        public async Task Handle_AddInstructor_Should_Return_StatusCode400_When_UploadFailed()
        {
            // Arrange
            var handler = new InstructorCommandHandler(_localizerMock.Object, _mapper, _instructorServiceMock.Object);
            var command = new AddInstructorCommand();
            _instructorServiceMock.Setup(x => x.AddInstructorAsync(It.IsAny<School.Domain.Entities.Instructor>(), It.IsAny<Microsoft.AspNetCore.Http.IFormFile>())).ReturnsAsync("FailedToUploadImage");
            _localizerMock.Setup(l => l[SharedResourcesKeys.FailedToUploadImage]).Returns(new LocalizedString(SharedResourcesKeys.FailedToUploadImage, "Upload Failed"));

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Upload Failed");
        }
        #endregion
    }
}
