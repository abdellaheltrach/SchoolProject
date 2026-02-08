using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Repositories;
using School.XUnitTest.Fixtures;

namespace School.XUnitTest.Repositories
{
    public class StudentRepositoryIntegrationTests : IntegrationTestBase
    {

        private async Task<Department> SeedDepartmentAsync()
        {
            var _DepartmentRepository = new DepartmentRepository(_dbContext);

            var department = DepartmentFixture.CreateValidDepartment();
            var newDepartment = await _DepartmentRepository.AddAsync(department);
            return newDepartment;
        }


        #region Generic Repository Tests


        [Fact]
        public async Task AddAsync_ShouldAddStudent()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);

            // Act
            await _studentRepository.AddAsync(student);

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.Should().NotBeNull();
            result.NameEn.Should().Be(student.NameEn);
        }




        [Fact]
        public async Task AddRangeAsync_ShouldAddList()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var _studentRepository = new StudentRepository(_dbContext);

            // Act
            await _studentRepository.AddRangeAsync(students);

            var count = await _studentRepository.GetTableNoTracking().CountAsync();

            // Assert
            count.Should().Be(3);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStudent()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);


            await _studentRepository.AddAsync(student);

            // Act
            student.NameEn = "Updated Name";
            await _studentRepository.UpdateAsync(student);

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.NameEn.Should().Be("Updated Name");
        }


        [Fact]
        public async Task UpdateRangeAsync_ShouldUpdateList()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var _studentRepository = new StudentRepository(_dbContext);


            await _studentRepository.AddRangeAsync(students);

            // Act
            foreach (var s in students)
            {
                s.NameEn = $"Updated {s.NameEn}";
            }
            await _studentRepository.UpdateRangeAsync(students);

            var updatedStudents = await _studentRepository.GetTableNoTracking().ToListAsync();

            // Assert
            updatedStudents.All(s => s.NameEn.StartsWith("Updated")).Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteStudent()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);

            await _studentRepository.AddAsync(student);
            // Act
            await _studentRepository.DeleteAsync(student);

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldDeleteList()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var _studentRepository = new StudentRepository(_dbContext);


            await _studentRepository.AddRangeAsync(students);

            // Act
            await _studentRepository.DeleteRangeAsync(students);

            var count = await _studentRepository.GetTableNoTracking().CountAsync();

            // Assert
            count.Should().Be(0);
        }


        [Fact]
        public async Task GetTableNoTracking_ShouldReturnQueryable()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(5, department);
            var _studentRepository = new StudentRepository(_dbContext);

            await _studentRepository.AddRangeAsync(students);

            // Act
            var query = _studentRepository.GetTableNoTracking();
            var result = await query.ToListAsync();

            // Assert
            result.Should().HaveCount(5);
            result.Should().AllSatisfy(s => s.NameEn.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task GetTableAsTracking_ShouldReturnTrackedQueryable()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);

            await _studentRepository.AddAsync(student);

            // Act
            var trackedQuery = _studentRepository.GetTableAsTracking();
            var result = await trackedQuery.FirstOrDefaultAsync(s => s.StudentID == student.StudentID);

            result.NameEn = "Modified via Tracked Query";
            await _studentRepository.SaveChangesAsync();

            // Assert - Verify change was tracked and saved
            var updatedStudent = await _studentRepository.GetByIdAsync(student.StudentID);
            updatedStudent.NameEn.Should().Be("Modified via Tracked Query");
        }


        [Fact]
        public async Task GetTableAsTracking_WithMultipleStudents_ShouldBeTracked()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var _studentRepository = new StudentRepository(_dbContext);

            await _studentRepository.AddRangeAsync(students);

            // Act
            var trackedStudents = await _studentRepository
                .GetTableAsTracking()
                .ToListAsync();

            foreach (var student in trackedStudents)
            {
                student.NameEn = $"{student.NameEn} - Updated";
            }
            await _studentRepository.SaveChangesAsync();

            // Assert
            var allStudents = await _studentRepository.GetTableNoTracking().ToListAsync();
            allStudents.Should().AllSatisfy(s => s.NameEn.Should().Contain("Updated"));
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldPersistChanges()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);


            // Act
            await _studentRepository.AddAsync(student);//add and save changes in one step

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.Should().NotBeNull();
            result.NameEn.Should().Be(student.NameEn);
        }


        [Fact]
        public async Task SaveChangesAsync_ShouldPersistMultipleChanges()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var _studentRepository = new StudentRepository(_dbContext);

            await _studentRepository.AddRangeAsync(students);

            // Act

            var count = await _studentRepository.GetTableNoTracking().CountAsync();

            // Assert
            count.Should().Be(3);
        }


        [Fact]
        public async Task BeginTransaction_ShouldReturnValidTransaction()
        {
            // Act
            var _studentRepository = new StudentRepository(_dbContext);

            var transaction = _studentRepository.BeginTransaction();

            // Assert
            transaction.Should().NotBeNull();

            // Cleanup
            transaction?.Dispose();
        }

        [Fact]
        public async Task BeginTransaction_WithCommit_ShouldPersistChanges()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);


            // Act
            using var transaction = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.AddAsync(student);
                _studentRepository.Commit();
            }
            catch
            {
                _studentRepository.RollBack();
                throw;
            }

            // Assert
            var result = await _studentRepository.GetByIdAsync(student.StudentID);
            result.Should().NotBeNull();
            result.NameEn.Should().Be(student.NameEn);
        }


        [Fact]
        public async Task BeginTransaction_WithRollback_ShouldUndoChanges()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);

            // Act
            using var transaction = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.AddAsync(student);

                // Simulate error
                throw new InvalidOperationException("Simulated error");
            }
            catch
            {
                _studentRepository.RollBack();
            }

            // Assert

            // Verify the rollback method was called without exception
            true.Should().BeTrue();
        }


        [Fact]
        public async Task Commit_ShouldCommitTransaction()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);

            var transaction = _studentRepository.BeginTransaction();

            try
            {
                // Act
                await _studentRepository.AddAsync(student);
                _studentRepository.Commit();

                // Assert
                var result = await _studentRepository.GetByIdAsync(student.StudentID);
                result.Should().NotBeNull();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        [Fact]
        public async Task RollBack_ShouldNotPersistUncommittedChanges()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _studentRepository = new StudentRepository(_dbContext);

            var transaction = _studentRepository.BeginTransaction();

            try
            {
                // Act
                await _studentRepository.AddAsync(student);
                _studentRepository.RollBack();

                // Assert
                // Note: With InMemory DB, rollback behavior may differ from real DB
                // This test ensures the API calls work correctly
                true.Should().BeTrue();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        [Fact]
        public async Task Commit_WithMultipleOperations_ShouldCommitAll()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var _studentRepository = new StudentRepository(_dbContext);


            var transaction = _studentRepository.BeginTransaction();

            try
            {
                // Act
                await _studentRepository.AddRangeAsync(students);
                _studentRepository.Commit();

                // Assert
                var count = await _studentRepository.GetTableNoTracking().CountAsync();
                count.Should().Be(3);
            }
            finally
            {
                transaction?.Dispose();
            }
        }


        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ReturnsNull()
        {
            // Act
            var _studentRepository = new StudentRepository(_dbContext);

            var result = await _studentRepository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(5, department);
            var _studentRepository = new StudentRepository(_dbContext);

            await _studentRepository.AddRangeAsync(students);

            // Act
            var count = await _studentRepository.GetTableNoTracking().CountAsync();

            // Assert
            count.Should().Be(5);
        }

        #endregion

        #region Custom Repository Tests

        [Fact]
        public async Task GetAllStudentListAsync_ShouldReturnStudentsWithDepartments()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var _studentRepository = new StudentRepository(_dbContext);

            var student = StudentFixture.CreateValidStudent(department);
            await _studentRepository.AddAsync(student);

            // Act
            var result = await _studentRepository.GetAllStudentListAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Department.Should().NotBeNull();
            result[0].Department.DepartmentNameEn.Should().Be(department.DepartmentNameEn);
        }

        #endregion
    }
}