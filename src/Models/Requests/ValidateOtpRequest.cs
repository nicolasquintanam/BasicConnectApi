using System.Text.Json.Serialization;

namespace BasicConnectApi.Models;

public class ValidateOtpRequest
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("otp_value")]
    public string? OtpValue { get; set; }
    [JsonPropertyName("context")]
    public string? Context { get; set; }
}