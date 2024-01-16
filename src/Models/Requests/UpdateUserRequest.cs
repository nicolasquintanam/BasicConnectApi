using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicConnectApi.Models;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "The 'first_name' field is required.")]
    [StringLength(100, ErrorMessage = "The 'first_name' field must be a string with a maximum length of 100.")]
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "The 'last_name' field is required.")]
    [StringLength(100, ErrorMessage = "The 'last_name' field must be a string with a maximum length of 100.")]
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "The 'email' field is required.")]
    [EmailAddress(ErrorMessage = "The 'email' field is not a valid e-mail address.")]
    [StringLength(255, ErrorMessage = "The 'email' field must be a string with a maximum length of 255.")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}