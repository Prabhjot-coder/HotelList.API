using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTOs;
using WebApplication4.Models;
using WebApplication4.Repositories;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _repo;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(IDepartmentRepository repo, ILogger<DepartmentsController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var items = await _repo.GetAllAsync();
        var dtos = items.Select(d => new DepartmentDto(d.DepartmentId, d.Name, d.IsActive, d.CreatedDate));
        return Ok(dtos);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var items = await _repo.GetActiveDepartments();
        var dtos = items.Select(d => new DepartmentDto(d.DepartmentId, d.Name, d.IsActive, d.CreatedDate));
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await _repo.GetByIdAsync(id);
        if (d is null) return NotFound();
        return Ok(new DepartmentDto(d.DepartmentId, d.Name, d.IsActive, d.CreatedDate));
    }

    [HttpPost]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        var dept = new Department { Name = dto.Name, IsActive = true };
        var created = await _repo.AddAsync(dept);
        var result = new DepartmentDto(created.DepartmentId, created.Name, created.IsActive, created.CreatedDate);
        return CreatedAtAction(nameof(GetById), new { id = created.DepartmentId }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
    {
        var dept = await _repo.GetByIdAsync(id);
        if (dept is null) return NotFound();
        dept.Name = dto.Name;
        dept.IsActive = dto.IsActive;
        await _repo.UpdateAsync(dept);
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
