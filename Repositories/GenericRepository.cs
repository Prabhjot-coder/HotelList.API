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

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

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

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
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

    public async Task<bool> ExistsAsync(int id) => await _dbSet.FindAsync(id) is not null;

    public async Task<int> CountAsync() => await _dbSet.CountAsync();
}
