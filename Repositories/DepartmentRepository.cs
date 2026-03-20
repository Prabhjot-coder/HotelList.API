using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;
namespace WebApplication4.Repositories;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(SchoolContext context) : base(context) { }

    public async Task<IEnumerable<Department>> GetActiveDepartments() =>
        await _dbSet.AsNoTracking().Where(d => d.IsActive == true).ToListAsync();
}
