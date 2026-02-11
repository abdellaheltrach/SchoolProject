using FluentAssertions;
using MockQueryable;
using Moq;
using School.Domain.Entities;
using School.Domain.Entities.Procedures;
using School.Domain.Entities.Views;
using School.Infrastructure.Repositories.Interfaces;
using School.Infrastructure.Repositories.Interfaces.Procedures;
using School.Infrastructure.Repositories.Interfaces.Views;
using School.Service.Services;
using School.Tests.Fixtures.Procedure;
using School.XUnitTest.Fixtures;

namespace School.Tests.Services
{
    public class DepartmentServiceTests
    {
        #region fields
        private readonly Mock<IDepartmentRepository> _departmentRepoMock;
        private readonly Mock<IViewRepository<DepartementTotalStudentView>> _viewRepoMock;
        private readonly Mock<IDepartmentStudentCountProcedureRepository> _procRepoMock;

        private readonly DepartmentService _service;

        #endregion

        #region constructor
        public DepartmentServiceTests()
        {
            _departmentRepoMock = new Mock<IDepartmentRepository>();
            _viewRepoMock = new Mock<IViewRepository<DepartementTotalStudentView>>();
            _procRepoMock = new Mock<IDepartmentStudentCountProcedureRepository>();

            _service = new DepartmentService(
                _departmentRepoMock.Object,
                _viewRepoMock.Object,
                _procRepoMock.Object
            );
        }
        #endregion



        #region IsDepartmentIdExist Tests

        [Fact]
        public async Task IsDepartmentIdExist_WithExistingDepartment_ReturnsTrue()
        {
            // ARRANGE
            var departmentList = DepartmentFixture.CreateDepartmentList(3);


            var mockQueryable = departmentList.BuildMock();

            _departmentRepoMock.Setup(x => x.GetTableNoTracking())
                               .Returns(mockQueryable);

            // Act
            var result = await _service.IsDepartmentIdExist(departmentList[0].Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsDepartmentIdExist_WithNonExistingDepartment_ReturnsFalse()
        {
            // ARRANGE
            var emptyList = new List<Department>();

            _departmentRepoMock
                .Setup(r => r.GetTableNoTracking())
                .Returns(emptyList.BuildMock());

            // ACT
            var result = await _service.IsDepartmentIdExist(999);

            // ASSERT
            result.Should().BeFalse();
        }

        #endregion


        #region GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger Tests
        [Fact]
        public async Task GetDepartmentByIdIncluding_WithValidId_ReturnsCompleteObjectGraph()
        {
            // ARRANGE
            var departmentId = 1;
            var department = DepartmentFixture.CreateValidDepartmentWithAllRelations(departmentId);
            var mockQueryable = new List<Department> { department }.BuildMock();

            _departmentRepoMock.Setup(x => x.GetTableNoTracking()).Returns(mockQueryable);

            // ACT
            var result = await _service.GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger(departmentId);

            // ASSERT
            result.Should().NotBeNull();
            result.Id.Should().Be(departmentId);

            // Check all includes in one go
            result.InstructorManager.Should().NotBeNull();
            result.Instructors.Should().NotBeEmpty();
            result.DepartmentSubjects.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetDepartmentByIdIncluding_WithNonExistingId_ReturnsNull()
        {
            // ARRANGE
            var mockQueryable = new List<Department>().BuildMock();
            _departmentRepoMock.Setup(x => x.GetTableNoTracking()).Returns(mockQueryable);

            // ACT
            var result = await _service.GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger(999);

            // ASSERT
            result.Should().BeNull();
        }
        #endregion



        #region GetViewDepartmentDataAsync Tests
        [Fact]
        public async Task GetViewDepartmentDataAsync_ShouldReturnViewData()
        {
            // Arrange
            var viewData = DepartmentTotalStudentViewFixture.CreateList();

            var mockQueryable = viewData.BuildMock();

            _viewRepoMock
                .Setup(x => x.GetTableNoTracking())
                .Returns(mockQueryable);



            // Act
            var result = await _service.GetViewDepartmentDataAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].TotalStudents.Should().Be(10);
        }
        #endregion

        #region GetDepartmentStudentCountProcs Tests
        [Fact]
        public async Task GetDepartmentStudentCountProcs_ShouldReturnRepositoryResult()
        {
            // Arrange
            var parameters = new DepartmentStudentCountProcedureParameters
            {
                DepartmentId = 0
            };

            var expected = DepartmentStudentCountProcedureFixture.CreateList();

            _procRepoMock
                .Setup(x => x.GetDepartmentStudentCountProcs(parameters))
                .ReturnsAsync(expected);


            // Act
            var result = await _service.GetDepartmentStudentCountProcs(parameters);

            // Assert
            result.Should().BeEquivalentTo(expected);

            _procRepoMock.Verify(
                x => x.GetDepartmentStudentCountProcs(parameters),
                Times.Once
            );
        }
        #endregion
    }
}
