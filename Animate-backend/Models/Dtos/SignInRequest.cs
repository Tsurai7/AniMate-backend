using System.ComponentModel.DataAnnotations;

namespace Animate_backend.Models.Dtos;

public record SignInRequest
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}