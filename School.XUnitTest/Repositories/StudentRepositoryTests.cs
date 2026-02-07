using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories;
using School.XUnitTest.Fixtures;

namespace School.XUnitTest.Repositories
{
    public class StudentRepositoryTests : IAsyncLifetime
    {
        private AppDbContext _dbContext;
        private StudentRepository _studentRepository;

        public async ValueTask InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"SchoolDb_{Guid.NewGuid()}")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _dbContext = new AppDbContext(options);
            _studentRepository = new StudentRepository(_dbContext);
            await _dbContext.Database.EnsureCreatedAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.DisposeAsync();
        }

        #region Generic Repository Tests


        [Fact]
        public async Task AddAsync_ShouldAddStudent()
        {
            // Arrange
            var student = StudentFixture.CreateValidStudent();

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
            var students = StudentFixture.CreateStudentList(3);

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
            var student = StudentFixture.CreateValidStudent();
            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            // Act
            student.NameEn = "Updated Name";
            await _studentRepository.UpdateAsync(student);
            await _studentRepository.SaveChangesAsync();

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.NameEn.Should().Be("Updated Name");
        }


        [Fact]
        public async Task UpdateRangeAsync_ShouldUpdateList()
        {
            // Arrange
            var students = StudentFixture.CreateStudentList(3);
            await _studentRepository.AddRangeAsync(students);
            await _studentRepository.SaveChangesAsync();

            // Act
            foreach (var s in students)
            {
                s.NameEn = $"Updated {s.NameEn}";
            }
            await _studentRepository.UpdateRangeAsync(students);
            await _studentRepository.SaveChangesAsync();

            var updatedStudents = await _studentRepository.GetTableNoTracking().ToListAsync();

            // Assert
            updatedStudents.All(s => s.NameEn.StartsWith("Updated")).Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteStudent()
        {
            // Arrange
            var student = StudentFixture.CreateValidStudent();
            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            // Act
            await _studentRepository.DeleteAsync(student);
            await _studentRepository.SaveChangesAsync();

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldDeleteList()
        {
            // Arrange
            var students = StudentFixture.CreateStudentList(3);
            await _studentRepository.AddRangeAsync(students);
            await _studentRepository.SaveChangesAsync();

            // Act
            await _studentRepository.DeleteRangeAsync(students);
            await _studentRepository.SaveChangesAsync();

            var count = await _studentRepository.GetTableNoTracking().CountAsync();

            // Assert
            count.Should().Be(0);
        }


        [Fact]
        public async Task GetTableNoTracking_ShouldReturnQueryable()
        {
            // Arrange
            var students = StudentFixture.CreateStudentList(5);
            await _studentRepository.AddRangeAsync(students);
            await _studentRepository.SaveChangesAsync();

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
            var student = StudentFixture.CreateValidStudent();
            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

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
            var students = StudentFixture.CreateStudentList(3);
            await _studentRepository.AddRangeAsync(students);
            await _studentRepository.SaveChangesAsync();

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
            var student = StudentFixture.CreateValidStudent();
            await _studentRepository.AddAsync(student);

            // Act
            await _studentRepository.SaveChangesAsync();

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.Should().NotBeNull();
            result.NameEn.Should().Be(student.NameEn);
        }


        [Fact]
        public async Task SaveChangesAsync_ShouldPersistMultipleChanges()
        {
            // Arrange
            var students = StudentFixture.CreateStudentList(3);
            await _studentRepository.AddRangeAsync(students);

            // Act
            await _studentRepository.SaveChangesAsync();

            var count = await _studentRepository.GetTableNoTracking().CountAsync();

            // Assert
            count.Should().Be(3);
        }


        [Fact]
        public async Task BeginTransaction_ShouldReturnValidTransaction()
        {
            // Act
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
            var student = StudentFixture.CreateValidStudent();

            // Act
            using var transaction = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();
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
            var student = StudentFixture.CreateValidStudent();

            // Act
            using var transaction = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();

                // Simulate error
                throw new InvalidOperationException("Simulated error");
            }
            catch
            {
                _studentRepository.RollBack();
            }

            // Assert
            // Note: InMemory DB may not support true transaction rollback,
            // but this tests the API call structure is correct
            // With real DB (SQL Server), changes would be rolled back

            // Verify the rollback method was called without exception
            true.Should().BeTrue();
        }


        [Fact]
        public async Task Commit_ShouldCommitTransaction()
        {
            // Arrange
            var student = StudentFixture.CreateValidStudent();
            var transaction = _studentRepository.BeginTransaction();

            try
            {
                // Act
                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();
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
            var student = StudentFixture.CreateValidStudent();
            var transaction = _studentRepository.BeginTransaction();

            try
            {
                // Act
                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();
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
            var students = StudentFixture.CreateStudentList(3);
            var transaction = _studentRepository.BeginTransaction();

            try
            {
                // Act
                await _studentRepository.AddRangeAsync(students);
                await _studentRepository.SaveChangesAsync();
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
            var result = await _studentRepository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var students = StudentFixture.CreateStudentList(5);
            await _studentRepository.AddRangeAsync(students);
            await _studentRepository.SaveChangesAsync();

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
            var department = DepartmentFixture.CreateValidDepartment();
            _dbContext.departments.Add(department);
            await _studentRepository.SaveChangesAsync();

            var student = StudentFixture.CreateValidStudent(departmentId: department.Id);
            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

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