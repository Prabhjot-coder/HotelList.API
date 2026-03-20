using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTOs;
using WebApplication4.Models;
using WebApplication4.Repositories;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseRepository _repo;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICourseRepository repo, ILogger<CoursesController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>Get all courses</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _repo.GetAllAsync();
        var dtos = courses.Select(c => new CourseDto(c.CourseId, c.CourseName, c.IsActive, c.TeacherId));
        return Ok(dtos);
    }

    /// <summary>Get active courses only</summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var courses = await _repo.GetActiveCourses();
        var dtos = courses.Select(c => new CourseDto(c.CourseId, c.CourseName, c.IsActive, c.TeacherId));
        return Ok(dtos);
    }

    /// <summary>Get course by ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c is null) return NotFound();
        return Ok(new CourseDto(c.CourseId, c.CourseName, c.IsActive, c.TeacherId));
    }

    /// <summary>Create a new course</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        var course = new Course { CourseName = dto.CourseName, TeacherId = dto.TeacherId, IsActive = true };
        var created = await _repo.AddAsync(course);
        var result = new CourseDto(created.CourseId, created.CourseName, created.IsActive, created.TeacherId);
        return CreatedAtAction(nameof(GetById), new { id = created.CourseId }, result);
    }

    /// <summary>Update a course</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course is null) return NotFound();
        course.CourseName = dto.CourseName;
        course.IsActive = dto.IsActive;
        course.TeacherId = dto.TeacherId;
        await _repo.UpdateAsync(course);
        return NoContent();
    }

    /// <summary>Delete a course</summary>
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
