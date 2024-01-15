namespace BasicConnectApi.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RegisterRequest
{
    [Required(ErrorMessage = "The 'first_name' field is required.")]
    [StringLength(100)]
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "The 'last_name' field is required.")]
    [StringLength(100)]
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "The 'email' field is required.")]
    [EmailAddress(ErrorMessage = "The 'email' field is not a valid e-mail address.")]
    [StringLength(255)]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    [Required(ErrorMessage = "The 'password' field is required.")]
    [RegularExpression(@"^[0-9a-fA-F]{64}$", ErrorMessage = "The 'password' field must be a SHA-256.")]
    public string? Password { get; set; }
}
