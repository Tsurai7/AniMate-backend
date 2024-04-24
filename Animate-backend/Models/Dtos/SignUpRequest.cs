using System.ComponentModel.DataAnnotations;

namespace Animate_backend.Models.Dtos;

public record SignUpRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
}