using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;
namespace WebApplication4.Repositories;
public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(SchoolContext context) : base(context) { }
    public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId) =>
        await _dbSet.AsNoTracking().Where(s => s.CourseId == courseId).ToListAsync();
}
