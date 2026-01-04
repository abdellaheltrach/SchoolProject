using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Bases
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task DeleteRangeAsync(ICollection<T> entities);
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
    }
}
