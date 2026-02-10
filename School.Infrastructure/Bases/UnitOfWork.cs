using Microsoft.EntityFrameworkCore.Storage;
using School.Infrastructure.Bases.Interfaces;
using School.Infrastructure.Context;

namespace School.Infrastructure.Bases
{


    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }


        #region Repository Management
        public IGenericRepositoryAsync<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepositoryAsync<>).MakeGenericType(type);
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepositoryAsync<T>)_repositories[type];
        }

        public TRepository CustomRepository<TRepository>() where TRepository : class
        {
            var type = typeof(TRepository);

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = Activator.CreateInstance(type, _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (TRepository)_repositories[type];
        }
        #endregion

        #region Transaction Management
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction?.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                _transaction?.DisposeAsync();
                _transaction = null;
                _repositories.Clear();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                await _transaction?.RollbackAsync();
            }
            finally
            {
                _transaction?.DisposeAsync();
                _transaction = null;
                _repositories.Clear();
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
            _repositories?.Clear();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }

            if (_context != null)
            {
                await _context.DisposeAsync();
            }

            _repositories?.Clear();
        }
        #endregion
    }
}
