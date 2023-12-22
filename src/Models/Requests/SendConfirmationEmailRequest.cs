namespace BasicConnectApi.Models;

using System.ComponentModel.DataAnnotations;

public class SendConfirmationEmailRequest
{
    [Required(ErrorMessage = "The 'email' field is required.")]
    [EmailAddress(ErrorMessage = "The 'email' field is not a valid e-mail address.")]
    [StringLength(255)]
    public string Email { get; set; }
}