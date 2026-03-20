using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication4.Data;

namespace WebApplication4.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly SchoolContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(SchoolContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression) =>
        await _dbSet.AsNoTracking().Where(expression).ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _dbSet.FindAsync(id) is not null;

    public async Task<int> CountAsync() =>
        await _dbSet.CountAsync();
}
