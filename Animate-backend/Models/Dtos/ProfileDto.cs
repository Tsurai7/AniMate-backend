namespace Animate_backend.Models.Dtos;

public record ProfileDto
(
    string Username,
    string ProfileImage,
    string Email,
    List<string> WatchedTitles,
    List<string> LikedTitles
);