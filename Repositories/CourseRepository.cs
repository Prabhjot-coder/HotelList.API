using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;
namespace WebApplication4.Repositories;
public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(SchoolContext context) : base(context) { }
    public async Task<IEnumerable<Course>> GetActiveCourses() =>
        await _dbSet.AsNoTracking().Where(c => c.IsActive == true).ToListAsync();
}
