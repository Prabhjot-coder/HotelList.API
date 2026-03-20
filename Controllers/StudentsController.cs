using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTOs;
using WebApplication4.Models;
using WebApplication4.Repositories;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _repo;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentRepository repo, ILogger<StudentsController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var students = await _repo.GetAllAsync();
        var dtos = students.Select(s => new StudentDto(s.StudentId, s.FirstMidName, s.LastName, s.Email, s.CourseId));
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s is null) return NotFound();
        return Ok(new StudentDto(s.StudentId, s.FirstMidName, s.LastName, s.Email, s.CourseId));
    }

    [HttpGet("by-course/{courseId:int}")]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCourse(int courseId)
    {
        var students = await _repo.GetStudentsByCourseAsync(courseId);
        var dtos = students.Select(s => new StudentDto(s.StudentId, s.FirstMidName, s.LastName, s.Email, s.CourseId));
        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
    {
        var student = new Student { FirstMidName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, CourseId = dto.CourseId };
        var created = await _repo.AddAsync(student);
        var result = new StudentDto(created.StudentId, created.FirstMidName, created.LastName, created.Email, created.CourseId);
        return CreatedAtAction(nameof(GetById), new { id = created.StudentId }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto)
    {
        var student = await _repo.GetByIdAsync(id);
        if (student is null) return NotFound();
        student.FirstMidName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;
        student.CourseId = dto.CourseId;
        await _repo.UpdateAsync(student);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _repo.ExistsAsync(id)) return NotFound();
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
