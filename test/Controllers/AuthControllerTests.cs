namespace BasicConnectApi.Test.Controllers;

using BasicConnectApi.Controllers;
using BasicConnectApi.Services;
using BasicConnectApi.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BasicConnectApi.Custom;
using Microsoft.AspNetCore.Http;

public class AuthControllerTests
{
    private readonly AuthController _controller;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<ILogger<AuthController>> _loggerServiceMock;

    public AuthControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _loggerServiceMock = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_userServiceMock.Object, _jwtServiceMock.Object, _loggerServiceMock.Object);
    }

    [Fact]
    public void Login_With_Valid_Credentials_Should_Return_OkResult_With_Token()
    {
        // Arrange
        var validLoginRequest = new LoginRequest { Email = "valid@example.com", Password = "ValidPassword" };
        var userId = It.IsAny<int>();
        var tokenGenerated = "dummyToken";
        _userServiceMock.Setup(x => x.AuthenticateUser(validLoginRequest.Email, validLoginRequest.Password, out userId)).Returns(true);
        _jwtServiceMock.Setup(x => x.GenerateToken(It.IsAny<string>(), Enums.TokenTypeEnum.AccessToken)).Returns(tokenGenerated);

        // Act
        var result = _controller.Login(validLoginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(okResult.Value);
        Assert.True(baseResponse.IsSuccess);
        Assert.NotNull(baseResponse.Data);
        var tokenProperty = baseResponse.Data.GetType().GetProperty("token");
        var tokenValue = tokenProperty?.GetValue(baseResponse.Data);
        Assert.NotNull(tokenValue);
        Assert.Equal(tokenValue, tokenGenerated);
    }

    [Fact]
    public void Login_With_Invalid_Credentials_Should_Return_UnauthorizedResult_With_Error_Message()
    {
        // Arrange
        var invalidLoginRequest = new LoginRequest { Email = "invalid@example.com", Password = "InvalidPassword" };
        var userId = It.IsAny<int>();
        _userServiceMock.Setup(x => x.AuthenticateUser(invalidLoginRequest.Email, invalidLoginRequest.Password, out userId)).Returns(false);

        // Act
        var result = _controller.Login(invalidLoginRequest);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(unauthorizedResult.Value);
        Assert.False(baseResponse.IsSuccess);
        Assert.Equal("Invalid username or password", baseResponse.Message);
    }

    [Fact]
    public void Verify_Logout_Method_Is_Decorated_With_Authorize_Attribute()
    {
        var methodInfo = _controller.GetType().GetMethod("Logout");
        var attributes = methodInfo?.GetCustomAttributes(typeof(AccessTokenAuthorizeAttribute), true);
        Assert.True(attributes?.Any(), "No AuthorizeAttribute found on Logout method");
    }

    [Fact]
    public void Verify_Logout_Method_Returns_Ok()
    {
        // Arrange
        var mockHttpContext = new Mock<HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.SetupGet(r => r.Headers).Returns(new HeaderDictionary { { "Authorization", "dummyToken" } });
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
        var controllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };
        _controller.ControllerContext = controllerContext;
        _jwtServiceMock.Setup(j => j.GetTokenFromAuthorizationHeader(mockRequest.Object.Headers)).Returns("dummyToken");
        _jwtServiceMock.Setup(x => x.RevokeToken("dummyToken"));

        // Act
        var result = _controller.Logout();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(okResult.Value);
        Assert.True(baseResponse.IsSuccess);
        Assert.NotNull(baseResponse.Data);
    }
}