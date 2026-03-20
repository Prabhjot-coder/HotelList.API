namespace WebApplication4.DTOs;

public record DepartmentDto(int DepartmentId, string? Name, bool? IsActive, DateTime? CreatedDate);
public record CreateDepartmentDto(string Name);
public record UpdateDepartmentDto(string Name, bool IsActive);
