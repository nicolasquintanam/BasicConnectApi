using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicConnectApi.Models;

public class UpdatePasswordRequest
{
    [JsonPropertyName("old_password")]
    [Required(ErrorMessage = "The 'old_password' field is required.")]
    [RegularExpression(@"^[0-9a-fA-F]{64}$", ErrorMessage = "The 'old_password' field must be a SHA-256.")]
    public string? OldPassword { get; set; }

    [JsonPropertyName("new_password")]
    [Required(ErrorMessage = "The 'new_password' field is required.")]
    [RegularExpression(@"^[0-9a-fA-F]{64}$", ErrorMessage = "The 'new_password' field must be a SHA-256.")]
    public string? NewPassword { get; set; }
}