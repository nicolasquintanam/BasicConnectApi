namespace BasicConnectApi.Test.Services;

using BasicConnectApi.Services;

public class TokenServiceTests
{
    [Fact]
    public void GenerateToken_Should_Return_Valid_Token()
    {
        // Arrange
        var tokenService = new TokenService();

        // Act
        var token = tokenService.GenerateToken(50);

        // Assert
        Assert.NotNull(token);
        Assert.Equal(50, token.Length); // Asegura que la longitud del token sea la esperada
        Assert.Matches("^[a-zA-Z0-9]*$", token); // Asegura que el token solo contiene caracteres alfanuméricos
    }
}
