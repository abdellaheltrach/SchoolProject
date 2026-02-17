using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Bases.GenericRepository;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepositoryAsync<Subject>, ISubjectRepository
    {
        #region Fields
        private readonly DbSet<Subject> _Departement;

        #endregion
        #region Constructors
        public SubjectRepository(AppDbContext context) : base(context)
        {
            _Departement = context.Set<Subject>();
        }
        #endregion
        #region Methods
        #endregion
    }
}
