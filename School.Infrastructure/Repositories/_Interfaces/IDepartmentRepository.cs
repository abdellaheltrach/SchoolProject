using School.Domain.Entities;
using School.Infrastructure.Bases.GenericRepository;

namespace School.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmentRepository : IGenericRepositoryAsync<Department>
    {
    }
}
