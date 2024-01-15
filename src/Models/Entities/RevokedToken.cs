namespace BasicConnectApi.Models;

public class RevokedToken
{
    public int Id { get; set; }
    public string? TokenId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}