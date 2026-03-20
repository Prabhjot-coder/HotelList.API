namespace WebApplication4.DTOs;

public record CourseDto(int CourseId, string? CourseName, bool? IsActive, int? TeacherId);
public record CreateCourseDto(string CourseName, int? TeacherId);
public record UpdateCourseDto(string CourseName, bool IsActive, int? TeacherId);
