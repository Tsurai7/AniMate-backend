namespace Animate_backend.Models.Dtos;

public record AuthResponse
(
    string AccessToken,
    string RefreshToken
);