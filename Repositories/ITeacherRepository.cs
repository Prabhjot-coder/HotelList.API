using WebApplication4.Models;
namespace WebApplication4.Repositories;
public interface ITeacherRepository : IGenericRepository<Teacher>
{
    Task<IEnumerable<Teacher>> GetTeachersByDepartmentAsync(int departmentId);
}
