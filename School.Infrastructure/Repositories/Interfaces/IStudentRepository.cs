using School.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Reposetries.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentListAsync();

    }
}
