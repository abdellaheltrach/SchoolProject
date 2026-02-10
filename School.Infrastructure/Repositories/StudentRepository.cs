using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Bases.GenericRepository;
using School.Infrastructure.Context;
using School.Infrastructure.Reposetries.Interfaces;


namespace School.Infrastructure.Repositories
{
    public class StudentRepository : GenericRepositoryAsync<Student>, IStudentRepository
    {
        #region Fields
        private readonly DbSet<Student> _Students;
        #endregion

        #region Constructors
        public StudentRepository(AppDbContext context): base(context) 
        {
            _Students = context.Set<Student>();
        }
        #endregion

        #region Methods
        public async Task<List<Student>> GetAllStudentListAsync()
        {
            return await _Students.Include(s=>s.Department).ToListAsync();
        }
        #endregion
    }
}
