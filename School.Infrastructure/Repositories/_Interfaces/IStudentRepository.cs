using School.Domain.Entities;
using School.Infrastructure.Bases.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Reposetries.Interfaces
{
    public interface IStudentRepository: IGenericRepositoryAsync<Student>
    {
        Task<List<Student>> GetAllStudentListAsync();

    }
}
