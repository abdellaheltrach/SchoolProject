using School.Infrastructure.Bases.GenericRepository;

namespace School.Infrastructure.Repositories.Interfaces.Views
{
    public interface IViewRepository<T> : IGenericRepositoryAsync<T> where T : class
    {
    }
}
