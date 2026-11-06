using System.ComponentModel.DataAnnotations;

namespace HardwareShop;

public class LoginRequest
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}