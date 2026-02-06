using Microsoft.AspNetCore.Http;
using School.Domain.Entities;

namespace School.Service.Services.Interfaces
{
    public interface IInstructorService
    {
        public Task<decimal> GetSalarySummationOfInstructor();
        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
        public Task<string> AddInstructorAsync(Instructor instructor, IFormFile file);

    }
}