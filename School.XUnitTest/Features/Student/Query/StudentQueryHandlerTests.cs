using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Base.ApiResponse;
using School.Core.Base.Wrappers;
using School.Core.Features.Students.Queries.Hundlers;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.QueriesResponse;
using School.Core.Features.Students.Queries.Response;
using School.Core.Resources;
using School.Domain.Entities;
using School.Service.Services.Interfaces;
using School.Core.Mapping.StudentMapping;
using System.Net;
using MockQueryable;
using MockQueryable.Moq;
using EntityStudent = School.Domain.Entities.Student;

namespace School.Tests.Features.Student.Query
{
    public class StudentQueryHandlerTest
    {
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly IMapper _mapperMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly StudentProfile _studentProfile;

        public StudentQueryHandlerTest()
        {
            _studentProfile = new();
            _studentServiceMock = new();
            _localizerMock = new();
            var configuration = new MapperConfiguration(c => c.AddProfile(_studentProfile));
            _mapperMock = new Mapper(configuration);
        }

        #region Get Student List Query
        [Fact]
        public async Task Handle_GetStudentList_Should_Return_StatusCode200()
        {
            // Arrange
            var handler = new StudentQueryHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var query = new GetStudentListQuery();
            var students = new List<EntityStudent> { new EntityStudent { StudentID = 1, NameEn = "Ahmed" } };
            _studentServiceMock.Setup(x => x.GetAllStudentListAsync()).ReturnsAsync(students);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().HaveCount(1);
            result.Data[0].StudentID.Should().Be(1);
            _studentServiceMock.Verify(x => x.GetAllStudentListAsync(), Times.Once);
        }
        #endregion

        #region Get Student By Id Query
        [Fact]
        public async Task Handle_GetStudentById_Should_Return_StatusCode200_When_Found()
        {
            // Arrange
            var handler = new StudentQueryHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var query = new GetStudentByIdQuery(1);
            var student = new EntityStudent { StudentID = 1, NameEn = "Ahmed" };
            _studentServiceMock.Setup(x => x.GetStudentByIdWithNoTrachingAsync(1)).ReturnsAsync(student);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.StudentID.Should().Be(1);
            _studentServiceMock.Verify(x => x.GetStudentByIdWithNoTrachingAsync(1), Times.Once);
        }

        [Fact]
        public async Task Handle_GetStudentById_Should_Return_StatusCode404_When_NotFound()
        {
            // Arrange
            var handler = new StudentQueryHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var query = new GetStudentByIdQuery(1);
            EntityStudent? student = null;
            _studentServiceMock.Setup(x => x.GetStudentByIdWithNoTrachingAsync(1)).ReturnsAsync((EntityStudent)student!);
            _localizerMock.Setup(l => l[SharedResourcesKeys.NotFound]).Returns(new LocalizedString(SharedResourcesKeys.NotFound, "Not Found"));

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _studentServiceMock.Verify(x => x.GetStudentByIdWithNoTrachingAsync(1), Times.Once);
        }
        #endregion

        #region Get Student Paginated List Query
        [Fact]
        public async Task Handle_GetStudentPaginatedList_Should_Return_StatusCode200()
        {
            // Arrange
            var handler = new StudentQueryHundler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var query = new GetStudentPaginatedListQuery { PageNumber = 1, PageSize = 10, Search = "Search", OrderBy = School.Domain.enums.StudentOrdringEnum.NameEn, SortDesc = true };
            var students = new List<EntityStudent> { new EntityStudent { StudentID = 1, NameEn = "Ahmed" } }.BuildMock();
            
            _studentServiceMock.Setup(x => x.FilterStudentPaginatedQuerable(query.Search, query.OrderBy, query.SortDesc))
                .Returns(students);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Data.Should().HaveCount(1);
            _studentServiceMock.Verify(x => x.FilterStudentPaginatedQuerable(query.Search, query.OrderBy, query.SortDesc), Times.Once);
        }
        #endregion
    }
}
