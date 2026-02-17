using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Entities.Procedures;
using School.Domain.Entities.Views;
using School.Infrastructure.Repositories.Interfaces;
using School.Infrastructure.Repositories.Interfaces.Procedures;
using School.Infrastructure.Repositories.Interfaces.Views;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        #region fields
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IViewRepository<DepartementTotalStudentView> _viewDepartmentRepository;
        private readonly IDepartmentStudentCountProcedureRepository _departmentStudentCountProcRepository;

        #endregion

        #region constructor
        public DepartmentService(IDepartmentRepository departmentRepository, IViewRepository<DepartementTotalStudentView> viewDepartmentRepository,
                                             IDepartmentStudentCountProcedureRepository departmentStudentCountProcRepository)
        {
            _departmentRepository = departmentRepository;
            _viewDepartmentRepository = viewDepartmentRepository;
            _departmentStudentCountProcRepository = departmentStudentCountProcRepository;

        }
        #endregion


        #region Methods
        public async Task<Department> GetDepartmentByIdIncluding_DS_Subj_Ins_InsManger(int id)
        {
            var Department = await _departmentRepository.GetTableNoTracking().Where(x => x.Id.Equals(id))
                                                        //in paginated list we dont need students list
                                                        //.Include(x => x.Students) 
                                                        .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subject)
                                                        .Include(x => x.Instructors)
                                                        .Include(x => x.InstructorManager).FirstOrDefaultAsync();
            return Department;
        }

        public async Task<bool> IsDepartmentIdExist(int departmentId)
        {
            return await _departmentRepository.GetTableNoTracking().AnyAsync(x => x.Id.Equals(departmentId));
        }

        public async Task<List<DepartementTotalStudentView>> GetViewDepartmentDataAsync()
        {
            var viewDepartment = await _viewDepartmentRepository.GetTableNoTracking().ToListAsync();
            return viewDepartment;
        }

        public async Task<IReadOnlyList<DepartmentStudentCountProcedure>> GetDepartmentStudentCountProcs(DepartmentStudentCountProcedureParameters parameters)
        {
            return await _departmentStudentCountProcRepository.GetDepartmentStudentCountProcs(parameters);
        }


        #endregion



    }
}
