using WebApplication4.Models;
namespace WebApplication4.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<IEnumerable<Department>> GetActiveDepartments();
}
