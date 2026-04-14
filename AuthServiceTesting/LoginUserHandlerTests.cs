using Auth_Service.Business_handlers;
using Auth_Service.Domain;
using Auth_Service.Entity;
using Auth_Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class LoginUserHandlerTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<ILogger<LoginUserHandler>> _loggerMock;
    private readonly LoginUserHandler _handler;

    public LoginUserHandlerTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _loggerMock = new Mock<ILogger<LoginUserHandler>>();
        _handler = new LoginUserHandler(_authRepositoryMock.Object, _loggerMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsInvalidMessage()
    {
        // Arrange
        var command = new LoginUserCommand { Username = "test", Password = "password" };
        _authRepositoryMock.Setup(r => r.GetUserByUsernameAsync("test"))
            .ReturnsAsync((Users)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Invalid username or password.", result.Message);
        Assert.Equal(string.Empty, result.Token);
    }

    [Fact]

 
    public async Task Handle_InvalidPassword_ReturnsInvalidMessage()
    {
        // Arrange
        var command = new LoginUserCommand { Username = "test", Password = "wrongpassword" };
        var user = new Users
        {
            Username = "test",
            PasswordHash = "correctpassword",
            RoleId = 1,
            Email = "test@example.com",
            Role = new Roles   // <-- Required navigation property initialized
            {
                RoleId = 1,
                RoleName = "Admin"
            }
        };

        _authRepositoryMock.Setup(r => r.GetUserByUsernameAsync("test"))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Invalid username or password.", result.Message);
        Assert.Equal(string.Empty, result.Token);
    }


    [Fact]
    public async Task Handle_RoleNotFound_ReturnsRoleNotFoundMessage()
    {
        // Arrange
        var command = new LoginUserCommand { Username = "test", Password = "correctpassword" };
        var user = new Users
        {
            Username = "test",
            PasswordHash = "correctpassword",
            RoleId = 1,
            Email = "test@example.com",
            Role = new Roles { RoleId = 1, RoleName = "Admin" } // required property initialized
        };

        _authRepositoryMock.Setup(r => r.GetUserByUsernameAsync("test"))
            .ReturnsAsync(user);
        _authRepositoryMock.Setup(r => r.GetRoleByIdAsync(1))
            .ReturnsAsync((Roles)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("User role not found.", result.Message);
        Assert.Equal(string.Empty, result.Token);
    }

    [Fact]
    public async Task Handle_ValidUserAndRole_ReturnsSuccessMessageAndToken()
    {
        // Arrange
        var command = new LoginUserCommand { Username = "test", Password = "correctpassword" };
        var user = new Users
        {
            Username = "test",
            PasswordHash = "correctpassword",
            RoleId = 1,
            Email = "test@example.com",
            Role = new Roles { RoleId = 1, RoleName = "Admin" } // required property initialized
        };

        var role = new Roles { RoleId = 1, RoleName = "Admin" };

        _authRepositoryMock.Setup(r => r.GetUserByUsernameAsync("test"))
            .ReturnsAsync(user);
        _authRepositoryMock.Setup(r => r.GetRoleByIdAsync(1))
            .ReturnsAsync(role);
        _jwtServiceMock.Setup(j => j.GenerateJwtToken(user, "Admin"))
            .Returns("fake-jwt-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Login successful", result.Message);
        Assert.Equal("fake-jwt-token", result.Token);
        Assert.Equal("Admin", result.RoleName);
    }


    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsInternalErrorMessage()
    {
        // Arrange
        var command = new LoginUserCommand { Username = "test", Password = "password" };
        _authRepositoryMock.Setup(r => r.GetUserByUsernameAsync("test"))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Login failed due to an internal error.", result.Message);
        Assert.Equal(string.Empty, result.Token);
    }
}
