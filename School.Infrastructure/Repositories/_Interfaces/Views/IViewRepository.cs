using School.Infrastructure.Bases.Interfaces;

namespace School.Infrastructure.Repositories.Interfaces.Views
{
    public interface IViewRepository<T> : IGenericRepositoryAsync<T> where T : class
    {
    }
}
