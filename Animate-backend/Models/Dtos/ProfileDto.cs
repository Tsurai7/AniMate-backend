namespace Animate_backend.Models.Dtos;

public record ProfileDto
{
    public string Username { get; set; }
    
    public string ProfileImage{ get; set; }
    
    public string Email { get; set; }

    public List<string> WatchedTitles { get; set; }
    
    public List<string> LikedTitles { get; set; }
}