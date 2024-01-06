using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicConnectApi.Models;

public class PasswordResetRequest
{
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "The 'password' field is required.")]
    [RegularExpression(@"^[0-9a-fA-F]{64}$", ErrorMessage = "The 'password' field must be a SHA-256.")]
    public string Password { get; set; }
}