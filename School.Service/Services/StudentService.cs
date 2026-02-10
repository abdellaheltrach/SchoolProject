using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.enums;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Reposetries.Interfaces;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class StudentService : IStudentService
    {
        #region fields
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion


        #region constructors
        public StudentService(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region methods
        public async Task<List<Student>> GetAllStudentListAsync()
        {
            return await _studentRepository.GetAllStudentListAsync();
        }

        public async Task<Student> GetStudentByIdWithNoTrachingAsync(int ID)
        {
            var student = _studentRepository.GetTableNoTracking().Include(s => s.Department).Where(s => s.StudentID.Equals(ID)).FirstOrDefault();
            return student;
        }

        public async Task<Student> GetStudentByIdWithTrachingAsync(int ID)
        {
            var student = await _studentRepository.GetByIdAsync(ID);
            return student;
        }

        public async Task<(bool success, string message)> AddStudentAsync(Student student)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                //student is already in the system 
                var existingStudent = _studentRepository.GetTableNoTracking().Where(s => s.NameEn.Equals(student.NameEn)).FirstOrDefault();
                if (existingStudent != null)
                {
                    return (false, "The student already exists!");
                }
                //Student not in the System
                var studentRepo = _unitOfWork.Repository<Student>();
                await studentRepo.AddAsync(student);
                await _unitOfWork.CommitAsync();
                return (true, "Student Added Successfully!");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return (false, "Failed to add student");
            }
        }

        public async Task<string> EditAsync(Student student)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var studentRepo = _unitOfWork.Repository<Student>();
                await studentRepo.UpdateAsync(student);
                await _unitOfWork.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            //Check if the name is Exist Or not
            var student = await _studentRepository.GetTableNoTracking().Where(x => x.NameAr.Equals(nameAr) & !x.StudentID.Equals(id)).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }

        public async Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
        {
            //Check if the name is Exist Or not
            var student = await _studentRepository.GetTableNoTracking().Where(x => x.NameAr.Equals(nameEn) & !x.StudentID.Equals(id)).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }

        public async Task<bool> DeleteAsync(Student student)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var studentRepo = _unitOfWork.Repository<Student>();

                await studentRepo.DeleteAsync(student);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public IQueryable<Student> FilterStudentPaginatedQuerable(string search, StudentOrdringEnum orderingBy, bool SortDesc)
        {
            var querable = _studentRepository.GetTableNoTracking().Include(x => x.Department).AsQueryable();
            if (search != null)
            {
                querable = querable.Where(x => x.NameAr.Contains(search) || x.NameEn.Contains(search) || x.Address.Contains(search));
            }



            switch (orderingBy)
            {
                case StudentOrdringEnum.StudentID:
                    querable = SortDesc ? querable.OrderByDescending(x => x.StudentID) : querable.OrderBy(x => x.StudentID);
                    break;
                case StudentOrdringEnum.NameAr:
                    querable = SortDesc ? querable.OrderByDescending(x => x.NameAr) : querable.OrderBy(x => x.NameAr);
                    break;
                case StudentOrdringEnum.NameEn:
                    querable = SortDesc ? querable.OrderByDescending(x => x.NameEn) : querable.OrderBy(x => x.NameEn);
                    break;
                case StudentOrdringEnum.Address:
                    querable = SortDesc ? querable.OrderByDescending(x => x.Address) : querable.OrderBy(x => x.Address);
                    break;
                case StudentOrdringEnum.DepartmentName:
                    querable = SortDesc ? querable.OrderByDescending(x => x.Department) : querable.OrderBy(x => x.Department);
                    break;
            }

            return querable;
        }

        public IQueryable<Student> GetStudentsByDepartmentIDQuerable(int DepartementId)
        {
            var query = _studentRepository.GetTableNoTracking().Where(st => st.DepartementId.Equals(DepartementId)).AsQueryable();
            return query;
        }




        #endregion




    }
}
