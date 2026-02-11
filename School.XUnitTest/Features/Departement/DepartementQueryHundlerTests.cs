using AutoMapper;
using Microsoft.Extensions.Localization;
using MockQueryable;
using Moq;
using School.Core.Features.Departement.Queries.Hundlers;
using School.Core.Features.Departement.Queries.Models;
using School.Core.Mapping.DepartmentMapping;
using School.Core.Resources;
using School.Domain.Entities;
using School.Domain.Entities.Procedures;
using School.Domain.Entities.Views;
using School.Service.Services.Interfaces;
using System.Net;

namespace School.Tests.Features.Departement
{
    public class DepartementQueryHundlerTests
    {
        private readonly Mock<IDepartmentService> _departmentServiceMock;
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly IMapper _mapper;

        public DepartementQueryHundlerTests()
        {
            _departmentServiceMock = new();
            _studentServiceMock = new();
            _localizerMock = new();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DepartementProfile>();
            });

            _mapper = config.CreateMapper();
        }

        #region GetDepartmentById

        [Fact]
        public async Task Handle_GetDepartmentById_Should_Return_404_When_NotFound()
        {
            // Arrange
            var handler = new DepartementQueryHundler(
                _departmentServiceMock.Object,
                _studentServiceMock.Object,
                _mapper,
                _localizerMock.Object);

            var query = new GetDepartementByIdQuery { ID = 1 };

            _departmentServiceMock
                .Setup(x => x.GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger(1))
                .ReturnsAsync((Department)null!);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.NotFound])
                .Returns(new LocalizedString(SharedResourcesKeys.NotFound, "Not Found"));

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("Not Found");
        }

        [Fact]
        public async Task Handle_GetDepartmentById_Should_Return_200_When_Found()
        {
            // Arrange
            var handler = new DepartementQueryHundler(
                _departmentServiceMock.Object,
                _studentServiceMock.Object,
                _mapper,
                _localizerMock.Object);

            var department = new Department
            {
                Id = 1,
                DepartmentNameEn = "IT",
                DepartmentNameAr = "تقنية"
            };

            var students = new List<Domain.Entities.Student>
            {
                new Domain.Entities.Student { StudentID = 1, NameEn = "Student 1", NameAr = "طالب 1" }
            }.BuildMock();

            _departmentServiceMock
                .Setup(x => x.GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger(1))
                .ReturnsAsync(department);

            _studentServiceMock
                .Setup(x => x.GetStudentsByDepartmentIDQuerable(1))
                .Returns(students);

            var query = new GetDepartementByIdQuery
            {
                ID = 1,
                StudentPageNumber = 1,
                StudentPageSize = 10
            };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
            result.Data.StudentList.Data.Should().HaveCount(1);
        }

        #endregion

        #region GetDepartmentStudentListCount

        [Fact]
        public async Task Handle_GetDepartmentStudentListCount_Should_Return_200()
        {
            // Arrange
            var handler = new DepartementQueryHundler(
                _departmentServiceMock.Object,
                _studentServiceMock.Object,
                _mapper,
                _localizerMock.Object);

            var viewData = new List<DepartementTotalStudentView>
            {
                new DepartementTotalStudentView()
            };

            _departmentServiceMock
                .Setup(x => x.GetViewDepartmentDataAsync())
                .ReturnsAsync(viewData);

            var query = new GetDepartmentStudentListCountQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        #endregion

        #region GetDepartmentStudentCountByID

        [Fact]
        public async Task Handle_GetDepartmentStudentCountByID_Should_Return_200()
        {
            // Arrange
            var handler = new DepartementQueryHundler(
                _departmentServiceMock.Object,
                _studentServiceMock.Object,
                _mapper,
                _localizerMock.Object);

            var procResult = new List<DepartmentStudentCountProcedure>
            {
                new DepartmentStudentCountProcedure()
            };

            _departmentServiceMock
                .Setup(x => x.GetDepartmentStudentCountProcs(It.IsAny<DepartmentStudentCountProcedureParameters>()))
                .ReturnsAsync(procResult);

            var query = new GetDepartmentStudentCountByIDQuery
            {
                DID = 1
            };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        #endregion
    }
}
