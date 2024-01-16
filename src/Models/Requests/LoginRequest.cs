namespace BasicConnectApi.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class LoginRequest
{
    [Required(ErrorMessage = "The 'email' field is required.")]
    [EmailAddress(ErrorMessage = "The 'email' field is not a valid e-mail address.")]
    [StringLength(255, ErrorMessage = "The 'email' field must be a string with a maximum length of 255.")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    [Required(ErrorMessage = "The 'password' field is required.")]
    [RegularExpression(@"^[0-9a-fA-F]{64}$", ErrorMessage = "The 'password' field must be a SHA-256.")]
    public string? Password { get; set; }
}
