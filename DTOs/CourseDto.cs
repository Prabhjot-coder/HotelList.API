namespace WebApplication4.DTOs;

public record CourseDto(int CourseId, string? Title, string? Description, bool? IsActive, int? TeacherId);
public record CreateCourseDto(string Title, string? Description, int? TeacherId);
public record UpdateCourseDto(string Title, string? Description, bool IsActive, int? TeacherId);
