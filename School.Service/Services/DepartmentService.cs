using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Repositories.Interfaces;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        #region fields
        private readonly IDepartmentRepository _departmentRepository;
        #endregion

        #region constructor
        public DepartmentService(IDepartmentRepository studentRepository)
        {
            _departmentRepository = studentRepository;
        }
        #endregion


        #region Methods
        public async Task<Department> GetDepartmentByIdIncluding_St_DS_Subj_Ins_InsManger(int id)
        {
            var Department = await _departmentRepository.GetTableNoTracking().Where(x => x.Id.Equals(id))
                                                        .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subject)
                                                        .Include(x => x.Instructors)
                                                        .Include(x => x.Students)
                                                        .Include(x => x.InstructorManager).FirstOrDefaultAsync();
            return Department;
        }
        #endregion
    }
}
