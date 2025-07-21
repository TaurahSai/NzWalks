using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;

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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
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
            existingEntity = entity;
            await dbContext.SaveChangesAsync();
            return existingEntity;
        }
    }
}
