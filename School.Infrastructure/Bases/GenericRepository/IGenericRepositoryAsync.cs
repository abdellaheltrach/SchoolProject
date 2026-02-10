namespace School.Infrastructure.Bases.GenericRepository
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        // Query Methods
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();

        // Add Methods
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);

        // Update Methods
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);

        // Delete Methods
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
    }
}
