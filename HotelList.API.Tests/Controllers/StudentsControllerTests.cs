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

public class StudentsControllerTests
{
    private readonly Mock<IStudentRepository> _repoMock;
    private readonly StudentsController _controller;

    public StudentsControllerTests()
    {
        _repoMock   = new Mock<IStudentRepository>();
        var logger  = new Mock<ILogger<StudentsController>>();
        _controller = new StudentsController(_repoMock.Object, logger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithAllStudents()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Student>
        {
            new() { StudentId = 1, FirstName = "Alice", LastName = "Brown", Email = "alice@t.com",
                    DateOfBirth = new DateOnly(2000,1,1), EnrollDate = new DateOnly(2022,9,1) },
            new() { StudentId = 2, FirstName = "Bob",   LastName = "Green", Email = "bob@t.com",
                    DateOfBirth = new DateOnly(2000,1,1), EnrollDate = new DateOnly(2022,9,1) }
        });

        var result = await _controller.GetAll();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeAssignableTo<IEnumerable<StudentDto>>()
          .Which.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenStudentExists()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(
            new Student { StudentId = 1, FirstName = "Alice", LastName = "B", Email = "a@t.com",
                          DateOfBirth = new DateOnly(2000,1,1), EnrollDate = new DateOnly(2022,9,1) });

        var result = await _controller.GetById(1);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Student?)null);

        var result = await _controller.GetById(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetByCourse_ReturnsOk_WithFilteredStudents()
    {
        _repoMock.Setup(r => r.GetStudentsByCourseAsync(5)).ReturnsAsync(new List<Student>
        {
            new() { StudentId = 3, FirstName = "Carol", LastName = "White", Email = "c@t.com",
                    CourseId = 5, DateOfBirth = new DateOnly(2000,1,1), EnrollDate = new DateOnly(2022,9,1) }
        });

        var result = await _controller.GetByCourse(5);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeAssignableTo<IEnumerable<StudentDto>>()
          .Which.Should().HaveCount(1);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithNewStudent()
    {
        var dto     = new CreateStudentDto("Tom", "Hanks", "tom@t.com", null);
        var student = new Student { StudentId = 5, FirstName = "Tom", LastName = "Hanks", Email = "tom@t.com",
                                    DateOfBirth = new DateOnly(2000,1,1), EnrollDate = new DateOnly(2022,9,1) };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Student>())).ReturnsAsync(student);

        var result = await _controller.Create(dto);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.ActionName.Should().Be(nameof(_controller.GetById));
        created.RouteValues!["id"].Should().Be(5);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenStudentExists()
    {
        var student = new Student { StudentId = 1, FirstName = "Old", LastName = "Name", Email = "old@t.com",
                                    DateOfBirth = new DateOnly(2000,1,1), EnrollDate = new DateOnly(2022,9,1) };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

        var result = await _controller.Update(1, new UpdateStudentDto("New", "Name", "new@t.com", null));

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenStudentMissing()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student?)null);

        var result = await _controller.Update(99, new UpdateStudentDto("X", "Y", "x@y.com", null));

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenStudentExists()
    {
        _repoMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenStudentMissing()
    {
        _repoMock.Setup(r => r.ExistsAsync(999)).ReturnsAsync(false);

        var result = await _controller.Delete(999);

        result.Should().BeOfType<NotFoundResult>();
    }
}
