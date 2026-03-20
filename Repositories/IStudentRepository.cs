using WebApplication4.Models;
namespace WebApplication4.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId);
}
