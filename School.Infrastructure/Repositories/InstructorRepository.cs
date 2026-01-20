using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class InstructorRepository : GenericRepositoryAsync<Instructor>, IInstructorRepository
    {
        #region Fields
        private readonly DbSet<Instructor> _Instructors;

        #endregion
        #region Constructors
        public InstructorRepository(AppDbContext context) : base(context)
        {
            _Instructors = context.Set<Instructor>();
        }
        #endregion
        #region Methods
        #endregion
    }
}
