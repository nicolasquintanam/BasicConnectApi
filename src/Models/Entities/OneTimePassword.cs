namespace BasicConnectApi.Models;

public class OneTimePassword
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public string? OtpValue { get; set; }
    public string? Context { get; set; }
    public DateTime ExpiryTime { get; set; }
    public bool IsUsed { get; set; }
}