namespace BasicConnectApi.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ConfirmEmailRequest
{
    [Required(ErrorMessage = "The 'email' field is required.")]
    [EmailAddress(ErrorMessage = "The 'email' field is not a valid e-mail address.")]
    [StringLength(255, ErrorMessage = "The 'email' field must be a string with a maximum length of 255.")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("token")]
    [Required(ErrorMessage = "The 'token' field is required.")]
    [RegularExpression(@"^[0-9a-zA-Z]{50}$", ErrorMessage = "The 'token' field is not valid.")]
    public string Token { get; set; }
}