namespace WebApplication4.DTOs;

public record TeacherDto(int TeacherId, string? FirstName, string? LastName, string? Email, bool? IsActive, int? DepartmentId);
public record CreateTeacherDto(string FirstName, string LastName, string Email, int? DepartmentId);
public record UpdateTeacherDto(string FirstName, string LastName, string Email, bool IsActive, int? DepartmentId);
