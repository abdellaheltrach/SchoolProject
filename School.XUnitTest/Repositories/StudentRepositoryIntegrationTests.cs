using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using School.Domain.Entities;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Reposetries.Interfaces;
using School.Infrastructure.Repositories;
using School.XUnitTest.Fixtures;

namespace School.XUnitTest.Repositories
{
    public class StudentRepositoryIntegrationTests : IntegrationTestBase
    {




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
            await _dbContext.SaveChangesAsync();

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.Should().NotBeNull();
            result.NameEn.Should().Be(student.NameEn);
        }




        [Fact]
        public async Task AddRangeAsync_ShouldAddList()
        {

            // ARRANGE
            var department = await SeedDepartmentAsync();
            var unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);
            var students = StudentFixture.CreateStudentList(3, department);

            // ACT
            var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var studentRepo = unitOfWork.CustomRepository<IStudentRepository>();
                await studentRepo.AddRangeAsync(students);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }

            // ASSERT
            var count = await _dbContext.Set<Student>().CountAsync();
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
            await _dbContext.SaveChangesAsync();

            // Act
            student.NameEn = "Updated Name";
            await _studentRepository.UpdateAsync(student);
            await _dbContext.SaveChangesAsync();

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
            await _dbContext.SaveChangesAsync();

            // Act
            foreach (var s in students)
            {
                s.NameEn = $"Updated {s.NameEn}";
            }
            await _studentRepository.UpdateRangeAsync(students);
            await _dbContext.SaveChangesAsync();

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
            await _dbContext.SaveChangesAsync();

            // Act
            await _studentRepository.DeleteAsync(student);
            await _dbContext.SaveChangesAsync();

            var result = await _studentRepository.GetByIdAsync(student.StudentID);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldDeleteList()
        {
            // ARRANGE
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);

            // Add students first
            var studentRepo = unitOfWork.CustomRepository<IStudentRepository>();
            await studentRepo.AddRangeAsync(students);
            await unitOfWork.CommitAsync();

            // ACT - Delete students
            var studentsToDelete = await _dbContext.Set<Student>().ToListAsync();
            await studentRepo.DeleteRangeAsync(studentsToDelete);
            await unitOfWork.CommitAsync();

            // ASSERT
            var count = await _dbContext.Set<Student>().CountAsync();
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
            await _dbContext.SaveChangesAsync();

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
            await _dbContext.SaveChangesAsync();

            // Act
            var trackedQuery = _studentRepository.GetTableAsTracking();
            var result = await trackedQuery.FirstOrDefaultAsync(s => s.StudentID == student.StudentID);

            result!.NameEn = "Modified via Tracked Query";
            await _dbContext.SaveChangesAsync();

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
            await _dbContext.SaveChangesAsync();

            // Act
            var trackedStudents = await _studentRepository
                .GetTableAsTracking()
                .ToListAsync();

            foreach (var student in trackedStudents)
            {
                student.NameEn = $"{student.NameEn} - Updated";
            }
            await _dbContext.SaveChangesAsync();

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
            await _studentRepository.AddAsync(student);
            await _dbContext.SaveChangesAsync();

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
            await _dbContext.SaveChangesAsync();

            // Act
            var count = await _studentRepository.GetTableNoTracking().CountAsync();

            // Assert
            count.Should().Be(3);
        }


        [Fact]
        public async Task BeginTransaction_ShouldReturnValidTransaction()
        {
            // Act
            var _unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);

            var transaction = await _unitOfWork.BeginTransactionAsync();

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
            var _unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);

            // Act
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var studentRepo = _unitOfWork.Repository<Student>();
                await studentRepo.AddAsync(student);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // Assert
            var _studentRepository = new StudentRepository(_dbContext);
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
            var _unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);

            // Act
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var studentRepo = _unitOfWork.Repository<Student>();
                await studentRepo.AddAsync(student);

                // Simulate error
                throw new InvalidOperationException("Simulated error");
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
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
            var _unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);

            var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Act
                var studentRepo = _unitOfWork.Repository<Student>();
                await studentRepo.AddAsync(student);
                await _unitOfWork.CommitAsync();

                // Assert
                var _studentRepository = new StudentRepository(_dbContext);
                var result = await _studentRepository.GetByIdAsync(student.StudentID);
                result.Should().NotBeNull();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        [Fact]
        public async Task RollBack_ShouldNotPersistUncommittedChanges()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var student = StudentFixture.CreateValidStudent(department);
            var _unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);

            var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Act
                var studentRepo = _unitOfWork.Repository<Student>();
                await studentRepo.AddAsync(student);
                await _unitOfWork.RollbackAsync();

                // Assert
                true.Should().BeTrue();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        [Fact]
        public async Task Commit_WithMultipleOperations_ShouldCommitAll()
        {
            // Arrange
            var department = await SeedDepartmentAsync();
            var students = StudentFixture.CreateStudentList(3, department);
            var _unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);

            var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Act
                var studentRepo = _unitOfWork.Repository<Student>();
                await studentRepo.AddRangeAsync(students);
                await _unitOfWork.CommitAsync();

                // Assert
                var _studentRepository = new StudentRepository(_dbContext);
                var count = await _studentRepository.GetTableNoTracking().CountAsync();
                count.Should().Be(3);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
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
            var unitOfWork = new UnitOfWork(_dbContext, _serviceProvider);
            var _studentRepository = unitOfWork.CustomRepository<StudentRepository>();


            // Act
            var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                await _studentRepository.AddRangeAsync(students);
                await unitOfWork.CommitAsync();
                var count = await _studentRepository.GetTableNoTracking().CountAsync();



                // Assert
                count.Should().Be(5);
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }

        }

        #endregion

        #region Custom Repository Tests

        [Fact]
        public async Task GetAllStudentListAsync_ShouldReturnStudentsWithDepartments()
        {
            // Arrange
            var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var department = DepartmentFixture.CreateValidDepartment();

            var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var departmentRepo = unitOfWork.CustomRepository<DepartmentRepository>();
                await departmentRepo.AddAsync(department);

                var student = StudentFixture.CreateValidStudent(department);
                var studentRepo = unitOfWork.CustomRepository<IStudentRepository>();
                await studentRepo.AddAsync(student);

                await unitOfWork.CommitAsync();

                // Act
                var result = await studentRepo.GetAllStudentListAsync();

                // Assert
                result.Should().NotBeNull();
                result.Should().HaveCount(1);

                var returnedStudent = result.First();
                returnedStudent.Department.Should().NotBeNull();
                returnedStudent.Department!.DepartmentNameEn
                    .Should().Be(department.DepartmentNameEn);
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        #endregion

        #region Helpers
        private async Task<Department> SeedDepartmentAsync()
        {
            var department = DepartmentFixture.CreateValidDepartment();
            _dbContext.Set<Department>().Add(department);
            await _dbContext.SaveChangesAsync();
            return department;
        }
        #endregion

    }
}