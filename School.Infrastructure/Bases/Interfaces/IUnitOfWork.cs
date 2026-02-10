using Microsoft.EntityFrameworkCore.Storage;

namespace School.Infrastructure.Bases.Interfaces
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
