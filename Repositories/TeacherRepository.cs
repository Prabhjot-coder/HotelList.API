using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;
namespace WebApplication4.Repositories;
public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(SchoolContext context) : base(context) { }
    public async Task<IEnumerable<Teacher>> GetTeachersByDepartmentAsync(int departmentId) =>
        await _dbSet.AsNoTracking().Where(t => t.DepartmentId == departmentId).ToListAsync();
}
