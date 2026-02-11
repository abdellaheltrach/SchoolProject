using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Base.ApiResponse;
using School.Core.Features.Students.Commands.Handlers;
using School.Core.Features.Students.Commands.Models;
using School.Core.Resources;
using School.Domain.Entities;
using School.Service.Services.Interfaces;
using School.Core.Mapping.StudentMapping;
using System.Net;
using EntityStudent = School.Domain.Entities.Student;

namespace School.Tests.Features.Student.Command
{
    public class StudentCommandHandlerTest
    {
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly IMapper _mapperMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly StudentProfile _studentProfile;

        public StudentCommandHandlerTest()
        {
            _studentProfile = new();
            _studentServiceMock = new();
            _localizerMock = new();
            var configuration = new MapperConfiguration(c => c.AddProfile(_studentProfile));
            _mapperMock = new Mapper(configuration);
        }

        #region Add Student Command
        [Fact]
        public async Task Handle_AddStudent_Should_Add_Data_And_StatusCode201()
        {
            // Arrange
            var handler = new StudentCommandHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var addStudentCommand = new AddStudentCommand() { NameAr = "محمد", NameEn = "Mohamed", Address = "Cairo", Phone = "0123456789", DepartementID = 1 };
            _studentServiceMock.Setup(x => x.AddStudentAsync(It.IsAny<EntityStudent>())).ReturnsAsync((true, "Success"));

            // Act
            var result = await handler.Handle(addStudentCommand, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Data.Should().Be("Success");
            _studentServiceMock.Verify(x => x.AddStudentAsync(It.IsAny<EntityStudent>()), Times.Once);
        }

        [Fact]
        public async Task Handle_AddStudent_Should_return_StatusCode400()
        {
            // Arrange
            var handler = new StudentCommandHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var addStudentCommand = new AddStudentCommand() { NameAr = "محمد", NameEn = "Mohamed", Address = "Cairo", Phone = "0123456789", DepartementID = 1 };
            _studentServiceMock.Setup(x => x.AddStudentAsync(It.IsAny<EntityStudent>())).ReturnsAsync((false, "Failed"));

            // Act
            var result = await handler.Handle(addStudentCommand, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed");
            _studentServiceMock.Verify(x => x.AddStudentAsync(It.IsAny<EntityStudent>()), Times.Once);
        }
        #endregion

        #region Edit Student Command
        [Fact]
        public async Task Handle_UpdateStudent_Should_Return_StatusCode404()
        {
            // Arrange
            var handler = new StudentCommandHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var updateStudentCommand = new EditStudentCommand() { StudentID = 6, NameAr = "محمد", NameEn = "Mohamed" };
            EntityStudent? student = null;
            _studentServiceMock.Setup(x => x.GetStudentByIdWithNoTrachingAsync(6)).ReturnsAsync(student);

            // Act
            var result = await handler.Handle(updateStudentCommand, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _studentServiceMock.Verify(x => x.GetStudentByIdWithNoTrachingAsync(6), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdateStudent_Should_Return_StatusCode200()
        {
            // Arrange
            var handler = new StudentCommandHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var updateStudentCommand = new EditStudentCommand() { StudentID = 1, NameAr = "احمد", NameEn = "Ahmed" };
            var student = new EntityStudent { StudentID = 1 };
            _studentServiceMock.Setup(x => x.GetStudentByIdWithNoTrachingAsync(1)).ReturnsAsync(student);
            _studentServiceMock.Setup(x => x.EditAsync(It.IsAny<EntityStudent>())).ReturnsAsync("Success");

            // Act
            var result = await handler.Handle(updateStudentCommand, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().Be("Updated Successfully");
            _studentServiceMock.Verify(x => x.EditAsync(It.IsAny<EntityStudent>()), Times.Once);
        }
        #endregion

        #region Delete Student Command
        [Fact]
        public async Task Handle_DeleteStudent_Should_Return_StatusCode404()
        {
            // Arrange
            var handler = new StudentCommandHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var deleteStudentCommand = new DeleteStudentCommand(6);
            EntityStudent? student = null;
            _studentServiceMock.Setup(x => x.GetStudentByIdWithTrachingAsync(6)).ReturnsAsync(student);

            // Act
            var result = await handler.Handle(deleteStudentCommand, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _studentServiceMock.Verify(x => x.GetStudentByIdWithTrachingAsync(6), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteStudent_Should_Return_StatusCode200()
        {
            // Arrange
            var handler = new StudentCommandHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var deleteStudentCommand = new DeleteStudentCommand(1);
            var student = new EntityStudent { StudentID = 1 };
            _studentServiceMock.Setup(x => x.GetStudentByIdWithTrachingAsync(1)).ReturnsAsync(student);
            _studentServiceMock.Setup(x => x.DeleteAsync(student)).ReturnsAsync(true);

            // Act
            var result = await handler.Handle(deleteStudentCommand, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            _studentServiceMock.Verify(x => x.DeleteAsync(student), Times.Once);
        }
        #endregion
    }
}
