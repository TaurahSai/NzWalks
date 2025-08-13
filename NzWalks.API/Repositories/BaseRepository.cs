using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;
using System.Linq.Expressions;

namespace NzWalks.API.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly NzWalksDbContext dbContext;

        public BaseRepository(NzWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> DeleteAsync(Guid id)
        {
            var existingEntity = await dbContext.Set<T>().FindAsync(id);
            if (existingEntity == null)
            {
                return null;
            }
            dbContext.Set<T>().Remove(existingEntity);
            await dbContext.SaveChangesAsync();
            return existingEntity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? orderBy = null,
            bool? isAscending = true, int pageNumber = 1,
            int pageSize = 1000, 
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (orderBy != null)
            {
                query = (isAscending ?? true) ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }

            var skip = (pageNumber - 1) * pageSize;

            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} was not found.");
            }
            return entity;
        }

        public async Task<T?> UpdateAsync(Guid id, T entity)
        {
            var existingEntity = await dbContext.Set<T>().FindAsync(id);
            if (existingEntity == null)
            {
                return null;
            }

            // Only update scalar and foreign key properties, not the key itself
            dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            await dbContext.SaveChangesAsync();

            // Reload navigation properties if needed
            foreach (var navigation in dbContext.Entry(existingEntity).Navigations)
            {
                if (!navigation.IsLoaded)
                    await navigation.LoadAsync();
            }

            return existingEntity;
        }
    }
}
