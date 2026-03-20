using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication4.Controllers;
using WebApplication4.DTOs;
using WebApplication4.Services;
using Xunit;

namespace HotelList.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<ITokenService> _tokenMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _tokenMock  = new Mock<ITokenService>();
        var logger  = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_tokenMock.Object, logger.Object);
    }

    [Fact]
    public void Register_ReturnsOk_ForNewUser()
    {
        var dto = new RegisterDto("John", "Doe", $"john_{Guid.NewGuid()}@test.com", "P@ssword1!");

        var result = _controller.Register(dto);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void Register_ReturnsBadRequest_WhenEmailDuplicated()
    {
        var email = $"dup_{Guid.NewGuid()}@test.com";
        _controller.Register(new RegisterDto("A", "B", email, "P@ssword1!"));

        var result = _controller.Register(new RegisterDto("C", "D", email, "P@ssword1!"));

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Login_ReturnsOkWithToken_WhenCredentialsValid()
    {
        var email = $"alice_{Guid.NewGuid()}@test.com";
        _controller.Register(new RegisterDto("Alice", "Smith", email, "P@ssword1!"));
        _tokenMock.Setup(s => s.GenerateToken(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns("mock.jwt.token");

        var result = _controller.Login(new LoginDto(email, "P@ssword1!"));

        var ok       = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = ok.Value.Should().BeOfType<AuthResponseDto>().Subject;
        response.Token.Should().Be("mock.jwt.token");
        response.Email.Should().Be(email);
    }

    [Fact]
    public void Login_ReturnsUnauthorized_WhenPasswordWrong()
    {
        var email = $"bob_{Guid.NewGuid()}@test.com";
        _controller.Register(new RegisterDto("Bob", "Jones", email, "Correct1!"));

        var result = _controller.Login(new LoginDto(email, "WrongPass"));

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public void Login_ReturnsUnauthorized_WhenEmailNotFound()
    {
        var result = _controller.Login(new LoginDto("nobody@nowhere.com", "SomePass"));

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public void Login_ReturnsCorrectFullName_InResponse()
    {
        var email = $"full_{Guid.NewGuid()}@test.com";
        _controller.Register(new RegisterDto("Jane", "Doe", email, "P@ssword1!"));
        _tokenMock.Setup(s => s.GenerateToken(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns("some.token");

        var result = _controller.Login(new LoginDto(email, "P@ssword1!"));

        var response = result.Should().BeOfType<OkObjectResult>().Subject
                             .Value.Should().BeOfType<AuthResponseDto>().Subject;
        response.FullName.Should().Be("Jane Doe");
    }

    [Fact]
    public void Login_ExpiresAt_IsInFuture()
    {
        var email = $"exp_{Guid.NewGuid()}@test.com";
        _controller.Register(new RegisterDto("Test", "User", email, "P@ssword1!"));
        _tokenMock.Setup(s => s.GenerateToken(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns("token");

        var result = _controller.Login(new LoginDto(email, "P@ssword1!"));

        var response = result.Should().BeOfType<OkObjectResult>().Subject
                             .Value.Should().BeOfType<AuthResponseDto>().Subject;
        response.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }
}
