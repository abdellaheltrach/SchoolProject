using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Context;
using School.Infrastructure.Reposetries.Interfaces;


namespace School.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion

        #region Constructors
        public StudentRepository(AppDbContext context)
        {
            this._context = context;
        }
        #endregion

        #region Methods
        public async Task<List<Student>> GetAllStudentListAsync()
        {
            return await _context.students.ToListAsync();
        }
        #endregion
    }
}
