using School.Domain.Entities.Procedures;

namespace School.Infrastructure.Repositories.Interfaces.Procedures
{
    public interface IDepartmentStudentCountProcedureRepository
    {
        public Task<IReadOnlyList<DepartmentStudentCountProcedure>> GetDepartmentStudentCountProcs(DepartmentStudentCountProcedureParameters parameters);
    }
}

