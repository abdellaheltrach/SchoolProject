using FluentAssertions;
using MockQueryable;
using Moq;
using School.Domain.Entities;
using School.Domain.Entities.Views;
using School.Infrastructure.Repositories.Interfaces;
using School.Infrastructure.Repositories.Interfaces.Procedures;
using School.Infrastructure.Repositories.Interfaces.Views;
using School.Service.Services;
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
            Assert.True(result);
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

    }
}
