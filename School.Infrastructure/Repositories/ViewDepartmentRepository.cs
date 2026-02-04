using Microsoft.EntityFrameworkCore;
using School.Domain.Entities.Views;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces.Views;

namespace School.Infrastructure.Repositories
{
    public class ViewDepartmentRepository : GenericRepositoryAsync<DepartementTotalStudentView>, IViewRepository<DepartementTotalStudentView>
    {
        #region Fields
        private DbSet<DepartementTotalStudentView> viewDepartment;
        #endregion

        #region Constructors
        public ViewDepartmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            viewDepartment = dbContext.Set<DepartementTotalStudentView>();
        }
        #endregion

        #region Handle Functions

        #endregion
    }
}
