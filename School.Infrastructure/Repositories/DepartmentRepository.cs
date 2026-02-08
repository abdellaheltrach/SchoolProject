using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepositoryAsync<Department>, IDepartmentRepository
    {
        #region Fields
        private readonly DbSet<Department> _Departement;
        #endregion

        #region Constructors
        public DepartmentRepository(AppDbContext context) : base(context)
        {
            _Departement = context.Set<Department>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
