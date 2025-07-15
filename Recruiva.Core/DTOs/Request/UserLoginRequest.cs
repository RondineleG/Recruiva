using System.ComponentModel.DataAnnotations;

namespace Recruiva.Web.DTOs.Request;

public class UserLoginRequest
{
    [Required(ErrorMessage = "The field {0} is mandatory")]
    [EmailAddress(ErrorMessage = "The field {0} is invalid")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is mandatory")]
    public string Password { get; set; }
}