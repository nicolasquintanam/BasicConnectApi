namespace BasicConnectApi.Test.Controllers;

using BasicConnectApi.Controllers;
using BasicConnectApi.Services;
using BasicConnectApi.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BasicConnectApi.Custom;
using Microsoft.AspNetCore.Http;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<ILogger<AuthController>> _loggerServiceMock;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _loggerServiceMock = new Mock<ILogger<AuthController>>();
        _controller = new UserController(_userServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Register_With_Valid_Parameters_Returns_Created()
    {
        // Arrange
        var validRegisterRequest = new RegisterRequest() { Email = "valid@example.com", FirstName = "Valid First Name", LastName = "Valid Last Name", Password = "Valid Password" };
        var userId = It.IsAny<int>();
        _userServiceMock.Setup(x => x.RegisterUser(validRegisterRequest.FirstName, validRegisterRequest.LastName, validRegisterRequest.Email, validRegisterRequest.Password, false)).Returns(userId);
        _userServiceMock.Setup(x => x.ExistsUser(validRegisterRequest.Email)).ReturnsAsync(false);

        // Act
        var result = await _controller.Register(validRegisterRequest);

        // Assert
        var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(createdResult.Value);
        Assert.True(baseResponse.IsSuccess);
        Assert.NotNull(baseResponse.Data);

        var idProperty = baseResponse.Data.GetType().GetProperty("user_id");
        var idValue = idProperty?.GetValue(baseResponse.Data);
        Assert.NotNull(idValue);
        Assert.Equal(idValue, userId);
    }

    [Fact]
    public async Task Register_With_Exists_User_Returns_Conflict()
    {
        // Arrange
        var validRegisterRequestWithExistsEmail = new RegisterRequest() { Email = "existsEmail@example.com", FirstName = "Valid First Name", LastName = "Valid Last Name", Password = "Valid Password" };
        _userServiceMock.Setup(x => x.ExistsUser(validRegisterRequestWithExistsEmail.Email)).ReturnsAsync(true);

        // Act
        var result = await _controller.Register(validRegisterRequestWithExistsEmail);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(conflictResult.Value);
        Assert.False(baseResponse.IsSuccess);
        Assert.Equal("The email is already registered", baseResponse.Message);
    }
}