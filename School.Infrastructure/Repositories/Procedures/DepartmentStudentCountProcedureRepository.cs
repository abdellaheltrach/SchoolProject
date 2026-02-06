using School.Domain.Entities.Procedures;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces.Procedures;
using StoredProcedureEFCore;


namespace School.Infrastructure.Repositories.Procedures
{
    public class DepartmentStudentCountProcedureRepository : IDepartmentStudentCountProcedureRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion
        #region Constructors
        public DepartmentStudentCountProcedureRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Handle Functions


        public async Task<IReadOnlyList<DepartmentStudentCountProcedure>> GetDepartmentStudentCountProcs(DepartmentStudentCountProcedureParameters parameters)
        {
            var rows = new List<DepartmentStudentCountProcedure>();

            await _context.LoadStoredProc(nameof(DepartmentStudentCountProcedure))
                .AddParam(nameof(DepartmentStudentCountProcedureParameters.DepartmentId), parameters.DepartmentId)
                .ExecAsync(async reader => rows = await reader.ToListAsync<DepartmentStudentCountProcedure>());

            return rows.AsReadOnly();
        }
        #endregion

    }
}