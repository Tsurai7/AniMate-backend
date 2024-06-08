using System.ComponentModel.DataAnnotations;

namespace Animate_backend.Models.Dtos;

public record SignUpRequest
(
    [Required]
    string Username,
    [Required]
    string Email,
    [Required]
    string Password
);