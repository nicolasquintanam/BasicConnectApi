namespace BasicConnectApi.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class GenerateOtpRequest
{
    [Required(ErrorMessage = "The 'email' field is required.")]
    [EmailAddress(ErrorMessage = "The 'email' field is not a valid e-mail address.")]
    [StringLength(255, ErrorMessage = "The 'email' field must be a string with a maximum length of 255.")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("context")]
    [Required(ErrorMessage = "The 'context' field is required.")]
    [EnumDataType(typeof(AcceptedContexts), ErrorMessage = "The context is not valid. It must be 'password_recovery' or 'confirm_email'.")]
    public string? Context { get; set; }
}