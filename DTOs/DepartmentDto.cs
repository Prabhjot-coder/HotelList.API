namespace WebApplication4.DTOs;

public record DepartmentDto(int DepartmentId, string? DepartmentName, bool? IsActive, DateTime? CreatedDate);
public record CreateDepartmentDto(string DepartmentName);
public record UpdateDepartmentDto(string DepartmentName, bool IsActive);
