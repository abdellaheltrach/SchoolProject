using School.Domain.Entities;
using School.Domain.Entities.Procedures;
using School.Domain.Entities.Views;

namespace School.Service.Services.Interfaces
{
    public interface IDepartmentService
    {
        public Task<Department> GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger(int id);
        public Task<bool> IsDepartmentIdExist(int departmentId);

        public Task<List<DepartementTotalStudentView>> GetViewDepartmentDataAsync();

        public Task<IReadOnlyList<DepartmentStudentCountProcedure>> GetDepartmentStudentCountProcs(DepartmentStudentCountProcedureParameters parameters);


    }
}
