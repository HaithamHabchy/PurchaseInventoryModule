

using DataAccessLayer.Data;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    // A generic repository class for managing entities in the database.
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        private readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _dbSet = db.Set<T>();
            _db = db;
        }

        // Retrieves a single entity that matches the given filter expression.
        // The query can optionally include related entities using the includeProperties parameter.
        // Returns null if no entity matches the filter.
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(filter);

            // If includeProperties is provided, include related entities
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        // Retrieves all entities that match the given filter expression.
        // The query can optionally include related entities using the includeProperties parameter.
        // Returns a list of matching entities, or an empty list if none match.
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(filter);

            // If includeProperties is provided, include related entities
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.ToListAsync();
        }



        // Adds a new entity to the database.
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Adds multiple new entities to the database.
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // Removes an existing entity from the database.
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Updates an existing entity in the database.
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
