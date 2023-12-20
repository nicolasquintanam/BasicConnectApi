namespace BasicConnectApi.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class LoginRequest
{
    [Required(ErrorMessage="The 'email' field is required.")]
    [EmailAddress(ErrorMessage="The 'email' field is not a valid e-mail address.")]
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("password")]
    [Required(ErrorMessage="The 'password' field is required.")]
    public string Password { get; set; }
}
