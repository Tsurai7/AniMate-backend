using System.ComponentModel.DataAnnotations;

namespace Animate_backend.Models.Dtos;

public record SignInRequest
(
    [Required]
    string Email,
    [Required]
    string Password
);