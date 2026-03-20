using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTOs;
using WebApplication4.Models;
using WebApplication4.Repositories;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly ITeacherRepository _repo;
    private readonly ILogger<TeachersController> _logger;

    public TeachersController(ITeacherRepository repo, ILogger<TeachersController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>Get all teachers</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TeacherDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var items = await _repo.GetAllAsync();
        var dtos = items.Select(t => new TeacherDto(t.TeacherId, t.FirstName, t.LastName, t.Email, t.IsActive, t.DepartmentId));
        return Ok(dtos);
    }

    /// <summary>Get teacher by ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var t = await _repo.GetByIdAsync(id);
        if (t is null) return NotFound();
        return Ok(new TeacherDto(t.TeacherId, t.FirstName, t.LastName, t.Email, t.IsActive, t.DepartmentId));
    }

    /// <summary>Get teachers by department</summary>
    [HttpGet("by-department/{departmentId:int}")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        var items = await _repo.GetTeachersByDepartmentAsync(departmentId);
        var dtos = items.Select(t => new TeacherDto(t.TeacherId, t.FirstName, t.LastName, t.Email, t.IsActive, t.DepartmentId));
        return Ok(dtos);
    }

    /// <summary>Create a new teacher</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto)
    {
        var teacher = new Teacher
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DepartmentId = dto.DepartmentId,
            HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsActive = true
        };
        var created = await _repo.AddAsync(teacher);
        var result = new TeacherDto(created.TeacherId, created.FirstName, created.LastName, created.Email, created.IsActive, created.DepartmentId);
        return CreatedAtAction(nameof(GetById), new { id = created.TeacherId }, result);
    }

    /// <summary>Update a teacher</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeacherDto dto)
    {
        var teacher = await _repo.GetByIdAsync(id);
        if (teacher is null) return NotFound();
        teacher.FirstName = dto.FirstName;
        teacher.LastName = dto.LastName;
        teacher.Email = dto.Email;
        teacher.IsActive = dto.IsActive;
        teacher.DepartmentId = dto.DepartmentId;
        await _repo.UpdateAsync(teacher);
        return NoContent();
    }

    /// <summary>Delete a teacher</summary>
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
