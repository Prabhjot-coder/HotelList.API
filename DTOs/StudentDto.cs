namespace WebApplication4.DTOs;

public record StudentDto(int StudentId, string? FirstName, string? LastName, string? Email, int? CourseId);
public record CreateStudentDto(string FirstName, string LastName, string Email, int? CourseId);
public record UpdateStudentDto(string FirstName, string LastName, string Email, int? CourseId);
