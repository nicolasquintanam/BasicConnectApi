namespace BasicConnectApi.Test.Controllers;

using BasicConnectApi.Controllers;
using BasicConnectApi.Services;
using BasicConnectApi.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;


public class AuthControllerTest
{
    private readonly AuthController _controller;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;

    public AuthControllerTest()
    {
        _userServiceMock = new Mock<IUserService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _controller = new AuthController(_userServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public void Login_With_Valid_Credentials_Should_Return_OkResult_With_Token()
    {
        // Arrange
        var validLoginRequest = new LoginRequest { Email = "valid@example.com", Password = "ValidPassword" };
        var userId = It.IsAny<int>();
        var tokenGenerated = "dummyToken";
        _userServiceMock.Setup(x => x.AuthenticateUser(validLoginRequest.Email, validLoginRequest.Password, out userId)).Returns(true);
        _jwtServiceMock.Setup(x => x.GenerateToken(It.IsAny<string>())).Returns(tokenGenerated);

        // Act
        var result = _controller.Login(validLoginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(okResult.Value);
        Assert.True(baseResponse.IsSuccess);
        Assert.NotNull(baseResponse.Data);
        var tokenProperty = baseResponse.Data.GetType().GetProperty("token");
        var tokenValue = tokenProperty.GetValue(baseResponse.Data);
        Assert.NotNull(tokenValue);
        Assert.Equal(tokenValue, tokenGenerated);
    }

}