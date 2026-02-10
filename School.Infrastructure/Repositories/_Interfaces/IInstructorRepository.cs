using School.Domain.Entities;
using School.Infrastructure.Bases.Interfaces;

namespace School.Infrastructure.Repositories.Interfaces
{
    public interface IInstructorRepository : IGenericRepositoryAsync<Instructor>
    {
    }
}
