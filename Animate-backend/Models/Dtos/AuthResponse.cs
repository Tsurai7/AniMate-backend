namespace Animate_backend.Models.Dtos;

public class AuthResponse
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}