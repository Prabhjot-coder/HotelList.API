using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication4.Controllers;
using WebApplication4.DTOs;
using WebApplication4.Models;
using WebApplication4.Repositories;
using Xunit;

namespace HotelList.API.Tests.Controllers;

public class CoursesControllerTests
{
    private readonly Mock<ICourseRepository> _repoMock;
    private readonly CoursesController _controller;

    public CoursesControllerTests()
    {
        _repoMock   = new Mock<ICourseRepository>();
        var logger  = new Mock<ILogger<CoursesController>>();
        _controller = new CoursesController(_repoMock.Object, logger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithAllCourses()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Course>
        {
            new() { CourseId = 1, CourseName = "Math 101",    IsActive = true },
            new() { CourseId = 2, CourseName = "Physics 101", IsActive = false }
        });

        var result = await _controller.GetAll();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeAssignableTo<IEnumerable<CourseDto>>()
          .Which.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetActive_ReturnsOnlyActiveCourses()
    {
        _repoMock.Setup(r => r.GetActiveCourses()).ReturnsAsync(new List<Course>
        {
            new() { CourseId = 1, CourseName = "Math 101", IsActive = true }
        });

        var result = await _controller.GetActive();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeAssignableTo<IEnumerable<CourseDto>>()
          .Which.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenCourseExists()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1))
                 .ReturnsAsync(new Course { CourseId = 1, CourseName = "Math 101", IsActive = true });

        var result = await _controller.GetById(1);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        var result = await _controller.GetById(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var dto    = new CreateCourseDto("Algebra", null);
        var course = new Course { CourseId = 10, CourseName = "Algebra", IsActive = true };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Course>())).ReturnsAsync(course);

        var result = await _controller.Create(dto);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenCourseExists()
    {
        var course = new Course { CourseId = 1, CourseName = "Old", IsActive = true };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(course);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Course>())).Returns(Task.CompletedTask);

        var result = await _controller.Update(1, new UpdateCourseDto("New", true, null));

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        _repoMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenMissing()
    {
        _repoMock.Setup(r => r.ExistsAsync(999)).ReturnsAsync(false);

        var result = await _controller.Delete(999);

        result.Should().BeOfType<NotFoundResult>();
    }
}
