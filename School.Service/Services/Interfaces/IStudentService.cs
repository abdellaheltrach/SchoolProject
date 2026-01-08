using School.Domain.Entities;

namespace School.Service.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentListAsync();
        Task<Student> GetStudentByIdWithNoTrachingAsync(int ID);
        Task<Student> GetStudentByIdWithTrachingAsync(int ID);
        Task<(bool success, string message)> AddStudentAsync(Student student);
        Task<string> EditAsync(Student student);

        Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
        Task<bool> DeleteAsync(Student student);



    }
}