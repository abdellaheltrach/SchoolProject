using Microsoft.EntityFrameworkCore;

namespace School.Core.Base.Wrappers
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber = 1, int pageSize = 10)
    where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            //checks for valid page number and size

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);

            int count = await source.AsNoTracking().CountAsync();
            if (count == 0) return PaginatedResult<T>.Success(new List<T>(), count, pageNumber, pageSize);
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
        }
    }
}
