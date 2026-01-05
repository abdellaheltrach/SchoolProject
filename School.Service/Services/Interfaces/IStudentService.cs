using School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Service.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentListAsync();
        Task<Student> GetStudentByIDAsync(int ID);
    }
}