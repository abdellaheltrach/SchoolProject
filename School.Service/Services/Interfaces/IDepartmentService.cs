using School.Domain.Entities;

namespace School.Service.Services.Interfaces
{
    public interface IDepartmentService
    {
        public Task<Department> GetDepartmentByIdIncluding_St_DS_Subj_Ins_InsManger(int id);

    }
}
