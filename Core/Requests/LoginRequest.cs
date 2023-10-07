using System.ComponentModel.DataAnnotations;

namespace EAD_APP.Core.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "User email is required")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}