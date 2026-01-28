using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos.User;
using ServiceBookingPlatform.Services;

namespace UnitTests;

public class UserLogInServiceTests
{
    private static readonly PasswordHasher<User> _passwordHasher = new();

    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_ValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        // Password must be 6-10 characters with uppercase, lowercase, digit, and special char
        var plainTextPassword = "Pass1!";
        var user = new User
        {
            FirstName = "Michael",
            LastName = "Richards",
            Email = "m_richards@gmail.com",
            PhoneNumber = "1234567890",
            PasswordHash = ""
        };
        
        // Hash the password
        user.PasswordHash = _passwordHasher.HashPassword(user, plainTextPassword);
        
        // Add user to database
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var loginRequest = new UserLogInRequestDto
        {
            Email = "m_richards@gmail.com",
            Password = plainTextPassword // Use plain text password for login
        };

        // Act
        var (success, errors) = await userLogInService.ValidateUserCredentialsAsync(loginRequest);

        // Assert
        Assert.True(success);
        Assert.Equal("Log in Successful", errors);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_InvalidPassword_ReturnsFailure()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        var plainTextPassword = "Pass1!";
        var user = new User
        {
            FirstName = "Michael",
            LastName = "Richards",
            Email = "m_richards@gmail.com",
            PhoneNumber = "1234567890",
            PasswordHash = ""
        };
        
        user.PasswordHash = _passwordHasher.HashPassword(user, plainTextPassword);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var loginRequest = new UserLogInRequestDto
        {
            Email = "m_richards@gmail.com",
            Password = "Wrong1!" // Wrong password but valid format
        };

        // Act
        var result = await userLogInService.ValidateUserCredentialsAsync(loginRequest);

        // Assert
        Assert.False(result.success);
        Assert.Equal("Password is incorrect", result.errors);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_UserNotFound_ReturnsFailure()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        var loginRequest = new UserLogInRequestDto
        {
            Email = "nonexistent@gmail.com",
            Password = "Pass1!"
        };

        // Act
        var result = await userLogInService.ValidateUserCredentialsAsync(loginRequest);

        // Assert
        Assert.False(result.success);
        Assert.Equal("User not found in database", result.errors);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_InvalidEmailFormat_ReturnsFailure()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        var loginRequest = new UserLogInRequestDto
        {
            Email = "invalid-email",
            Password = "Pass1!"
        };

        // Act
        var result = await userLogInService.ValidateUserCredentialsAsync(loginRequest);

        // Assert
        Assert.False(result.success);
        Assert.Contains("Email address is not in a valid format", result.errors);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_EmptyEmail_ReturnsFailure()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        var loginRequest = new UserLogInRequestDto
        {
            Email = "",
            Password = "Pass1!"
        };

        // Act
        var result = await userLogInService.ValidateUserCredentialsAsync(loginRequest);

        // Assert
        Assert.False(result.success);
        Assert.Contains("Email is required", result.errors);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_WeakPassword_ReturnsFailure()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        var loginRequest = new UserLogInRequestDto
        {
            Email = "test@gmail.com",
            Password = "weak" // Weak password
        };

        // Act
        var result = await userLogInService.ValidateUserCredentialsAsync(loginRequest);

        // Assert
        Assert.False(result.success);
        Assert.Contains("Password must be 6-10 characters", result.errors);
    }

    [Fact]
    public async Task ValidateUserDto_ValidUser_ReturnsValid()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        var validUser = new UserLogInRequestDto
        {
            Email = "test@example.com",
            Password = "Valid1!"
        };

        // Act
        var result = userLogInService.ValidateUserDto(validUser);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ValidateUserDto_InvalidEmail_ReturnsInvalid()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var userLogInService = new UserLogInService(context);
        
        var invalidUser = new UserLogInRequestDto
        {
            Email = "not-an-email",
            Password = "Valid1!"
        };

        // Act
        var result = userLogInService.ValidateUserDto(invalidUser);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Email address is not in a valid format", result.Errors);
    }
}
