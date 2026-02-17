using Microsoft.EntityFrameworkCore.Storage;
using School.Infrastructure.Bases.GenericRepository;

namespace School.Infrastructure.Bases.UnitOfWork
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IGenericRepositoryAsync<T> Repository<T>() where T : class;
        TRepository CustomRepository<TRepository>() where TRepository : class;


        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
