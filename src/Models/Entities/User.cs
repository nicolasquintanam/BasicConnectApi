namespace BasicConnectApi.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public List<RevokedToken> RevokedTokens { get; set; }
    public List<OneTimePassword> OneTimePasswords { get; set; }
}
