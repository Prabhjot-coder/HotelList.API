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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _repo.GetAllAsync();
        var dtos = courses.Select(c => new CourseDto(c.CourseId, c.Title, c.Description, c.IsActive, c.TeacherId));
        return Ok(dtos);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var courses = await _repo.GetActiveCourses();
        var dtos = courses.Select(c => new CourseDto(c.CourseId, c.Title, c.Description, c.IsActive, c.TeacherId));
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c is null) return NotFound();
        return Ok(new CourseDto(c.CourseId, c.Title, c.Description, c.IsActive, c.TeacherId));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        var course = new Course { Title = dto.Title, Description = dto.Description, TeacherId = dto.TeacherId, IsActive = true };
        var created = await _repo.AddAsync(course);
        var result = new CourseDto(created.CourseId, created.Title, created.Description, created.IsActive, created.TeacherId);
        return CreatedAtAction(nameof(GetById), new { id = created.CourseId }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course is null) return NotFound();
        course.Title = dto.Title;
        course.Description = dto.Description;
        course.IsActive = dto.IsActive;
        course.TeacherId = dto.TeacherId;
        await _repo.UpdateAsync(course);
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
