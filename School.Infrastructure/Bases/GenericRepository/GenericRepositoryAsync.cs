using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Context;

namespace School.Infrastructure.Bases.GenericRepository
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        #region Fields
        protected readonly AppDbContext _dbContext;

        #endregion

        #region Constructor(s)
        public GenericRepositoryAsync(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion



        #region Query Actions
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetTableNoTracking()
        {
            return _dbContext.Set<T>().AsNoTracking().AsQueryable();
        }

        public IQueryable<T> GetTableAsTracking()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
        #endregion

        #region Add Actions 
        public virtual async Task AddRangeAsync(ICollection<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

            return entity;
        }
        #endregion

        #region Update Actions - REMOVED SaveChangesAsync()
        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);

        }

        public virtual async Task UpdateRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);

        }
        #endregion

        #region Delete Actions
        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

        }

        public virtual async Task DeleteRangeAsync(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }

        }
        #endregion
    }
}
